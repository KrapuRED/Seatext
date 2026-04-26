using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class PlayerContex : FishContex
{
    public PlayerFish playerFish;

    public bool IsActiveFish;
}

public class PlayerFish : Fish, IPausable, IEatable
{
    public static PlayerFish playerFish { get; private set; }

    [Header("Player Fish Config")]
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float maxHealth;
    [SerializeField] private bool _isActiveFish;

    [Header("Fish System")]
    [SerializeField] private FishHunger _playerFishStatus;
    [SerializeField] private FishHealth _playerFishHealth;

    [Header("Events")]
    [SerializeField] private SetPositionPlayerEventSO setPositionPlayerEvent;

    private Rigidbody2D _rb2d;

    public FoodSize foodSize { get ; set ; }
    public FishHunger playerFishStatus => _playerFishStatus;

    private void Awake()
    {
        if (playerFish != null && playerFish != this)
        {
            Destroy(gameObject);
            return;
        }

        playerFish = this;
    }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();

        FishMovement.IntilizaFishMovement(_rb2d, FishData);
        PauseManager.instance.Register(this);
        _playerFishHealth.SetFishHealth(maxHealth);

        Contex = new PlayerContex
        {
            playerFish      = this,
            fishObject      = gameObject,
            fishEyeSight    = FishEyeSight,
            fishMovement    = FishMovement,
            fishSpeed       = FishSpeed,
            fishMouth       = FishMouth,
            IsActiveFish    = false
        };

        FishEyeSight.isCanSee = true;
        foodSize = FishData.fishSize;
    }

    private void Update()
    {
        _playerFishStatus.Starve();

        if (targetPosition == null)
            return;

        if (CheckDistanceToTarget() <= distanceToTarget)
        {
            targetPosition = null;
            return;
        }

        FishMovement.MoveFish(targetPosition, FishSpeed.GetFishSpeed(1));
    }

    private void OnEnable()
    {
        setPositionPlayerEvent.Register(SetPlayerFishDirection);
    }

    private void OnDisable()
    {
        setPositionPlayerEvent.Unregister(SetPlayerFishDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEatable eatable = collision.GetComponent<IEatable>();

        if (eatable == null)
            return;

        eatable.Eat(FishType);
    }

    public void TakingDamage(float damageValue)
    {
        if (_playerFishHealth.IsDead())
        {
            Debug.Log($"[PlayerFish - TakingDamage] PlayerFish {gameObject.name} has been killed!");
            gameObject.SetActive(false);
            return;
        }

        _playerFishHealth.OnTakeDamage(damageValue);
    }

    public override void SetBeenHunted(bool isBeenHunted, Fish c)
    {
        base.SetBeenHunted(isBeenHunted);
        Debug.Log($"{gameObject.name} is been Hunted!");
    }

    public override void DodgeAttackFish(Vector2 attackDirection)
    {
        base.DodgeAttackFish(attackDirection);
        Vector2 dodgeDir = Vector2.Perpendicular(attackDirection);
        FishMovement.Dodge(dodgeDir);
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

    public void OnPause()
    {
        enabled = false;
    }

    public void OnResume()
    {
        enabled = true;
    }

    public void Eat(FishType fishType)
    {
        Debug.Log($"[PlayerFish - Eat] Enemy Fish {gameObject.name} has been eaten!");
        switch (fishType)
        {
            case FishType.Big:
                TakingDamage(10);
                break;
        }
    }

    public void SetActiveFish(bool condition)
    {
        _isActiveFish = condition;
    }
}
