using UnityEngine;

public class FishMouth : MonoBehaviour
{
    public FishType ownerFishType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"[FishMouth - OnTriggerEnter2D] {gameObject.name} has entered the trigger with {collision.gameObject.name}");
        if (collision.TryGetComponent(out IEatable eatAble))
        {
            GameEvents.OnEatableEntered.Invoke(eatAble);
        }
    }
}
