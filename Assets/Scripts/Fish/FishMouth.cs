using UnityEngine;

public class FishMouth : MonoBehaviour
{
    public FishType ownerFishType;
    [SerializeField] private bool _isMouthOpen;

    public void SetMouthState(bool isOpen)
    {
        _isMouthOpen = isOpen;
        Debug.Log($"[FishMouth] SetMouthState({isOpen}) called by: {new System.Diagnostics.StackTrace()}", gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"[FishMouth - OnTriggerEnter2D] {gameObject.name} has entered the trigger with {collision.gameObject.name}");
        Debug.Log($"[FishMoth({gameObject.name}) - OnTriggerEnter2D] IsMouthOpen : {_isMouthOpen}");

        if (!_isMouthOpen)
        {
            Debug.Log($"[FishMoth({gameObject.name}) - OnTriggerEnter2D] Mouth is closed, ignoring collision.");
            return;
        }

        if (collision.TryGetComponent(out IEatable eatAble) && _isMouthOpen)
        {
            Debug.Log("Calling Events!");
            //GameEvents.OnEatableEntered.Invoke(eatAble);
        }
    }
}
