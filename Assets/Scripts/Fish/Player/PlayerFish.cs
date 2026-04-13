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
    [SerializeField] private float maxHunger;
    [SerializeField] private float currentHunger;
    [SerializeField] private float trashGain;

    [Header("UI")]
    [SerializeField] private StatusBarUI statusHungerUI;
    [SerializeField] private StatusHealthUI statusHealthUI;

    [Header("Events")]
    [SerializeField] private SetPositionPlayerEventSO setPositionPlayerEvent;

    private Rigidbody2D _rb2d;

    public FoodSize foodSize { get ; set ; }

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

        currentHunger = maxHunger;
        statusHungerUI.UpdateStatusBar(trashGain, maxHunger);

        currentHealth = maxHealth;
        statusHealthUI.UpdateStatusBar(currentHealth, maxHealth);

        fishEyeSight.isCanSee = true;
        foodSize = fishData.fishSize;
    }

    private void Update()
    {
        Starve();

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

    private void Starve()
    {
        if (currentHunger <= 0)
        {
            Debug.Log($"[PlayerFish - Update] PlayerFish {gameObject.name} is too hungry to move!");
            float damageValue = Time.deltaTime;
            TakingDamage(damageValue);
            return;
        }

        currentHunger -= Time.deltaTime;
        statusHungerUI.UpdateStatusBar(trashGain, currentHunger);
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
        statusHealthUI.UpdateStatusBar(currentHealth, maxHealth);
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

    public void SetTrashinHungerbar(float gainTrash)
    {
        trashGain += gainTrash;
        maxHunger -= gainTrash;

        ResetHunggerBar();
    }

    public void ResetHunggerBar()
    {
        statusHungerUI.UpdateStatusBar(trashGain, maxHunger);
        currentHunger = maxHunger;
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
