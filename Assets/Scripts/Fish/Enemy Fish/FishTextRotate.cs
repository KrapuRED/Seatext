using UnityEngine;

public class FishTextRotate : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    public void RotateCanvasUI(float deg)
    {
        float currentZRotation = deg * -1;

        //Debug.Log($"[FishTextRotate - RotateCanvasUI] Current Z Rotation : {currentZRotation}");

        _rectTransform.rotation = Quaternion.Euler(0, 0, currentZRotation);
    }
}
