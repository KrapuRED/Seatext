using UnityEngine;

public class FishMouth : MonoBehaviour
{
    public Fish ownerFish;
    [SerializeField] private bool _isMouthOpen;

    public void SetMouthState(bool isOpen)
    {
        _isMouthOpen = isOpen;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isMouthOpen)
        {
            Debug.Log($"[FishMoth({gameObject.name}) - OnTriggerEnter2D] Mouth is closed, ignoring collision.");
            return;
        }

        if (collision.TryGetComponent(out IEatable eatAble) && _isMouthOpen)
        {
            //Debug.Log($"[FishMoth({gameObject.name}) - OnTriggerEnter2D] Detected eatable object: {collision.gameObject.name}. Invoking OnEatableEntered event.");
            GameEvents.OnEatableEntered.Invoke(eatAble, ownerFish.FishType, ownerFish.FishIndex);
        }
    }
}
