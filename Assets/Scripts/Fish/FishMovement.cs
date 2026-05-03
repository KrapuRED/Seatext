using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("FishMovement Config")]
    [SerializeField] private float _dodgeForce;
    [SerializeField] private bool isCanMove;

    [Header("Effector")]
    [SerializeField] private FishTextRotate _fishTextRotate;

    [SerializeField] private Rigidbody2D _rigidbody2D;

    public void IntilizaFishMovement(Rigidbody2D rb2d, FishSO fishData)
    {
        _rigidbody2D = rb2d;

    }


    public void MoveFish(Transform TargetPosition, float speed ,bool runAway = false)
    {
        if (!isCanMove)
        {
            Debug.Log($"[FishMovement - MoveFish] Fish Cannot Move To Position!");
            return;
        }

        if (runAway)
        {
            speed *= 2f;
            Debug.Log($"New speed : {speed}");
        }

        RotateFish(TargetPosition);

        //Debug.Log($"[FishMovement - MoveFish] Target Position : {TargetPosition.position}, Fish Speed : {speed}");
        _rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, TargetPosition.position, speed * Time.deltaTime));
    }

    public void Dodge(Vector2 dodgeDir)
    {
        _rigidbody2D.AddForce(dodgeDir * _dodgeForce, ForceMode2D.Impulse);
    }

    public void RotateFish(Transform TargetPosition)
    {
        Vector2 direction = TargetPosition.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _rigidbody2D.rotation = angle - 90f;

        if (_fishTextRotate == null)
        {
            return;
        }

        _fishTextRotate.KeepTextUpright();
    }

    public void SetCanMove(bool canMove)
    {
        isCanMove = canMove;
    }
}
