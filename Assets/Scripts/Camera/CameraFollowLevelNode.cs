using System;
using System.Collections;
using UnityEngine;

public class CameraFollowLevelNode : MonoBehaviour
{
    [Header("Camera Follow Configuration")]
    [SerializeField] private float offsetCamera;
    [SerializeField] private float delayCamera;
    
    [SerializeField] private Camera _cam;
    private bool isReady;

    private void Awake()
    {
        _cam = Camera.main;
        isReady = _cam != null;
    }
    
    #region  Event Data
    private void OnEnable()
    {
        GameEvents.OnChangeCameraPosition.RemoveListener(SetCameraPosition);
        GameEvents.OnChangeCameraPosition.AddListener(SetCameraPosition);
    }

    private void OnDisable()
    {
        GameEvents.OnChangeCameraPosition.RemoveListener(SetCameraPosition);
    }

    private void OnDestroy()
    {
        GameEvents.OnChangeCameraPosition.RemoveListener(SetCameraPosition);
        _cam = null;
    }
    #endregion

    private void SetCameraPosition(Transform currentNodeTransform)
    {
        if (this == null)
            return;

        if (!isReady)
        {
            Debug.Log($"[CameraFollowLevelNode - SetCameraPosition] is not ready!");
            return;
        }

        if (_cam == null)
        {
            Debug.LogWarning($"[CameraFollowLevelNode - SetCameraPosition] _cam is null!");
            return;
        }

        Vector3 positinCam =  currentNodeTransform.position;
        Vector3 positionCamOffest = new Vector3(positinCam.x + offsetCamera, positinCam.y, -10);

        //Debug.Log($"[CameraFollowLevelNode - SetCameraPosition] is change position to {positionCamOffest}");
        _cam.transform.position = positionCamOffest;
    }
}
