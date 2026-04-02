using UnityEngine;

public class PlayerFish : Fish
{
    [Header("Player Fish Config")]
    [SerializeField] private Transform targetPosition;

    [Header("Events")]
    [SerializeField] private SetPositionPlayerEventSO setPositionPlayerEvent;

    private Rigidbody2D _rb2d;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        fishMovement.IntilizaFishMovement(_rb2d, fishData);
    }

    private void Update()
    {
        if (targetPosition == null)
            return;

        if (CheckDistanceToTarget() <= 0.01f)
        {
            targetPosition = null;
            return;
        }

        fishMovement.MoveFish(targetPosition);
    }

    public void SetPlayerFishDirection(Transform targetPosition)
    {
        Debug.Log("[PlayerFish - SetPlayerFishDirection] Try to Move PlayerFish");
        this.targetPosition = targetPosition;
    }

    private float CheckDistanceToTarget()
    {
        float distance = Vector3.Distance(transform.position, targetPosition.position);
        return distance;
    }

    private void OnEnable()
    {
        setPositionPlayerEvent.Register(SetPlayerFishDirection);
    }

    private void OnDisable()
    {
        setPositionPlayerEvent.Unregister(SetPlayerFishDirection);
    }

}
