using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FishEyeSight : MonoBehaviour
{
    [Header("Eye Fish Config")]
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    [SerializeField] private Transform EyePosition;
    [SerializeField] private LayerMask visionLayerMask;
    [SerializeField] private string dectactionTag;
    [SerializeField] private float limitFrame;

    [SerializeField] private List<Collider2D> _objectsInSights = new List<Collider2D>();
    [SerializeField] private Transform _currentObject;

    public bool isCanSee { private get; set; }
    public Vector2 AttackDirection {get; private set;}
    public Transform currentObject => _currentObject;


    public void UpdateEyeSight()
    {
        if (!isCanSee)
            return;

        if (Time.frameCount % limitFrame != 0)
            return; 

        Debug.Log("Update Eye Sight");
        Transform eyeResult = CheckEyeSight();

        if (_currentObject == null)
            _currentObject = eyeResult;

        bool isSeeing = _currentObject != null;

        if (isSeeing)
        {
            SetBeenHunted(isSeeing);
        }

    }

    private Transform CheckEyeSight()
    {
        //Direction Eye Facing
        Vector2 directionEye = EyePosition.up;
        OnDrawEyeSight(directionEye);
        _objectsInSights = Physics2D.OverlapCircleAll(transform.position, viewDistance, visionLayerMask).ToList();

        foreach (Collider2D targetCollider in _objectsInSights)
        {
            Transform target = targetCollider.transform;
            Vector2 distanceTotarget = (target.position - EyePosition.position).normalized;

            if (Vector2.Angle(directionEye, distanceTotarget) < viewAngle / 2)
            {
                RaycastHit2D hit = Physics2D.Raycast(EyePosition.position, distanceTotarget, viewDistance, visionLayerMask);

                if (hit.collider != null)
                {
                    return target;
                }
            }
        }

        return null;
    }

    private void SetBeenHunted(bool hunted)
    {
        if (_currentObject == null) return;

        Fish currentFish = _currentObject.GetComponent<Fish>();
        if (currentFish != null)
        {
            AttackDirection = (_currentObject.position - transform.position).normalized;
            currentFish.SetBeenHunted(hunted, currentFish);
        }
    }

    private void OnDrawGizmos()
    {
        if (EyePosition == null)
            return;

        Vector2 directionEye = EyePosition.up;
    }

    public void OnDrawEyeSight(Vector2 directionEye)
    {
        //Set Both Left and Right Boundary of Eye Sight
        Vector3 leftBoundary = Quaternion.AngleAxis(-viewAngle / 2, Vector3.forward) * directionEye;
        Vector3 rightBoundary = Quaternion.AngleAxis(viewAngle / 2, Vector3.forward) * directionEye;

        //Set Both Left and Right End Point of Eye Sight
        Vector3 leftEndPoint = EyePosition.position + leftBoundary * viewDistance;
        Vector3 rightEndPoint = EyePosition.position + rightBoundary * viewDistance;

        Debug.DrawLine(EyePosition.position, leftEndPoint, Color.red);
        Debug.DrawLine(EyePosition.position, rightEndPoint, Color.red);
        Debug.DrawLine(leftEndPoint, rightEndPoint, Color.green);
    }
}
