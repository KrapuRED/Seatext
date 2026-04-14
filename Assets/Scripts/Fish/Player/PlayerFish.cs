using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerFish : Fish, IPausable, IEatable
{
    public static PlayerFish playerFish { get; private set; }

    [Header("Player Fish Config")]
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [Header("Fish System")]
    [SerializeField] private FishStatus _playerFishStatus;

    [Header("Events")]
    [SerializeField] private SetPositionPlayerEventSO setPositionPlayerEvent;

    private Rigidbody2D _rb2d;

    public FoodSize foodSize { get ; set ; }
    public FishStatus playerFishStatus => _playerFishStatus;

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

        fishMovement.IntilizaFishMovement(_rb2d, fishData);
        PauseManager.instance.Register(this);

        currentHealth = maxHealth;

        fishEyeSight.isCanSee = true;
        foodSize = fishData.fishSize;
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

        fishMovement.MoveFish(targetPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEatable eatable = collision.GetComponent<IEatable>();

        if (eatable == null)
            return;

        eatable.Eat(fishType);
    }


    public void TakingDamage(float damageValue)
    {
        if (currentHealth <= 0)
        {
            Debug.Log($"[PlayerFish - TakingDamage] PlayerFish {gameObject.name} has been killed!");
            gameObject.SetActive(false);
            return;
        }

        currentHealth -= damageValue;
       _playerFishStatus.OnUpdateHealthBar(currentHealth, maxHealth);
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
        fishMovement.Dodge(dodgeDir);
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
        TakingDamage(10);
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
