using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("FishMovement Config")]
    [SerializeField] private float _speedFish;
    [SerializeField] private bool isCanMove;

    private Rigidbody2D _rigidbody2D;

    public void IntilizaFishMovement(Rigidbody2D rb2d, FishOS fishData)
    {
        _rigidbody2D = rb2d;
        _speedFish = fishData.speedFish;
    }


    public void MoveFish(Transform TargetPosition)
    {
        if (!isCanMove)
        {
            Debug.Log($"[FishMovement - MoveFish] Fish Cannot Move To Position!");
            return;
        }

        Debug.Log($"[FishMovement - MoveFish] Target Position : {TargetPosition.name}");
        _rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, TargetPosition.position, _speedFish * Time.deltaTime));
    }

    public void SetCanMove(bool canMove)
    {
        isCanMove = canMove;
    }
}
