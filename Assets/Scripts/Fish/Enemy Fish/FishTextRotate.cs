using UnityEngine;

public class FishTextRotate : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;

    public void KeepTextUpright()
    {
        _rectTransform.rotation = Quaternion.identity;
    }
}
