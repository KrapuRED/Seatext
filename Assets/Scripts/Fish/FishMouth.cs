using UnityEngine;

public class FishMouth : MonoBehaviour
{
    public FishType ownerFishType;

    public bool IsMouthOpen { get;  set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"[FishMouth - OnTriggerEnter2D] {gameObject.name} has entered the trigger with {collision.gameObject.name}");
        Debug.Log($"[FishMoth({gameObject.name}) - OnTriggerEnter2D] IsMouthOpen : {IsMouthOpen}");

        if (!IsMouthOpen)
        {
            Debug.Log($"[FishMoth({gameObject.name}) - OnTriggerEnter2D] Mouth is closed, ignoring collision.");
            return;
        }

        if (collision.TryGetComponent(out IEatable eatAble))
        {
            GameEvents.OnEatableEntered.Invoke(eatAble);
        }
    }
}
