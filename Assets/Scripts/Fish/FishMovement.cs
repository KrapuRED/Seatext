using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private Fish _ownerFish;
    
    [Header("FishMovement Config")]
    [SerializeField] private float _dodgeForce;
    [SerializeField] private bool isCanMove;
    [SerializeField] private float _rotationSpeed;
    
    [Header("Effector")]
    [SerializeField] private FishTextRotate _fishTextRotate;

    [SerializeField] private Rigidbody2D _rigidbody2D;

    public void IntilizaFishMovement(Rigidbody2D rb2d, FishSO fishData)
    {
        _rigidbody2D = rb2d;

    }


    public void MoveFish(Transform TargetPosition, float distance ,float speed ,bool runAway = false)
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
        _rigidbody2D.MovePosition(
            Vector2.MoveTowards(_rigidbody2D.position, 
                TargetPosition.position,
                speed * Time.fixedDeltaTime));
    }

    public void Dodge(Vector2 dodgeDir)
    {
        _rigidbody2D.AddForce(dodgeDir * _dodgeForce, ForceMode2D.Impulse);
    }

    public void RotateFish(Transform TargetPosition)
    {
        Vector2 direction = TargetPosition.position - transform.position;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float smoothAngle = Mathf.LerpAngle(_rigidbody2D.rotation, targetAngle, _rotationSpeed * Time.deltaTime);
        _rigidbody2D.rotation = smoothAngle;

        if (_fishTextRotate != null)
        {
            _fishTextRotate.KeepTextUpright();
        }
    }

    public void SetCanMove(bool canMove)
    {
        isCanMove = canMove;
    }
}
