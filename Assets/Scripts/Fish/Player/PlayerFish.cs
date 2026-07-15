using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class PlayerContex : FishContex
{
    public PlayerFish playerFish;
    public Transform RoamingPoint;

    public bool IsActiveFish;
    public bool IsIdle;
    public bool IsRoaming;
    public bool CanEatFish;
}

public class PlayerFish : Fish, IPausable, IEatable
{
    public static PlayerFish playerFish { get; private set; }

    [Header("Player Fish Config")]
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float maxHealth;
    [SerializeField] private bool _isActiveFish;
    [SerializeField] private float durationInvisible;
    [SerializeField] private bool _isInvisible;
    private float currentInvisible;

    [Header("Fish System")]
    [SerializeField] private FishHunger _playerFishHunger;
    [SerializeField] private FishHealth _playerFishHealth;
    [SerializeField] private StateMachine _stateMachine;

    private Rigidbody2D _rb2d;
    private Transform _moveTarget;
    private float _moveSpeed;

    public FoodSize foodSize { get ; set ; }
    public FishHunger PlayerFishHunger => _playerFishHunger;
    private PlayerContex PlayerContex => Contex as PlayerContex;

    private void Awake()
    {
        if (playerFish != null && playerFish != this)
        {
            Destroy(gameObject);
            return;
        }

        playerFish = this;
        
        StatusPlayerManager.Instance.InitializedStatus(_playerData);
    }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        
        FishMovement.IntilizaFishMovement(_rb2d, FishData);
        PauseManager.instance.Register(this);

        Contex = new PlayerContex
        {
            playerFish      = this,
            RoamingPoint    =  targetPosition,
            fishObject      = gameObject,
            fishEyeSight    = FishEyeSight,
            fishMovement    = FishMovement,
            fishSpeed       = FishSpeed,
            fishMouth       = FishMouth,
            IsActiveFish    = true
        };

        FishEyeSight.isCanSee = true;
        FishSpeed.ownerFishType = FishType;
        FishSpeed.InitiliazeFishSpeed(_playerData.baseFishStats.speedFish);
        
        FishMouth.ownerFish = this;
        FishMouth.SetMouthState(true);
        foodSize = FishData.fishSize;
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerActive.AddListener(SetActiveFish);
        GameEvents.OnDodgeAttackFish.AddListener(DodgeAttackFish);
        GameEvents.OnSetPositionPlayerEvent.AddListener(SetPlayerFishDirection);
        GameEvents.OnPlayerEating.AddListener(PlayerEating);
        
        GameEvents.OnEatableEntered.AddListener(HandleEating);
        
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerActive.RemoveListener(SetActiveFish);
        GameEvents.OnDodgeAttackFish.RemoveListener(DodgeAttackFish);
        GameEvents.OnSetPositionPlayerEvent.AddListener(SetPlayerFishDirection);
        GameEvents.OnPlayerEating.RemoveListener(PlayerEating);
        
        GameEvents.OnEatableEntered.RemoveListener(HandleEating);
        
    }

    private void OnDestroy()
    {
        PauseManager.instance.Unregister(this);
    }

    private void Update()
    {
        if (GameManager.instance.LevelDone)
            return;
        
        if (_isInvisible && currentInvisible > 0)
        {
            currentInvisible -= Time.deltaTime;
        }
        else
        {
            _isInvisible = false;
        }
    
        _playerFishHunger.Starve();

        if (targetPosition == null)
            return;

        if (PlayerContex.IsRoaming)
        {
            _moveTarget = targetPosition;
            _moveSpeed = FishSpeed.GetRoamingFishSpeed();
            return;
        }

        _moveTarget = targetPosition;
        _moveSpeed = FishSpeed.GetChaseFishSpeed();
        FishAnimation.OnHandlingMovementAnimation(CheckDistanceToTarget());
    }
    
    private void FixedUpdate()
    {
        if (_moveTarget == null)
            return;
            
        if (CheckDistanceToTarget() <= distanceToTarget && PlayerContex.IsIdle && !PlayerContex.IsRoaming)
        {
            targetPosition = null;
            _moveTarget = null; // also clear this, or FixedUpdate keeps using stale target
            return;
        }
    
        FishMovement.MoveFish(_moveTarget, CheckDistanceToTarget(), _moveSpeed);
    }

    private void ApplyDamage(float damageValue)
    {
        if (_playerFishHealth.IsDead())
        {
            Debug.Log($"[PlayerFish - TakingDamage] PlayerFish {gameObject.name} has been killed!");
            GameManager.instance.LevelNodeFailed();
            gameObject.SetActive(false);
            return;
        }

        _playerFishHealth.OnTakeDamage(damageValue);
    }
    
    public void TakingDamage(float damageValue)
    {
        if (_isInvisible)
            return;

        _isInvisible = true;
        currentInvisible = durationInvisible;
        
        ApplyDamage(damageValue);
    }
    
    public void TakeStarvationDamage(float damageValue)
    {
        Debug.Log($"Player taking starvation damage {damageValue}");
        
        // No invisibility check — starvation ticks every frame regardless
        ApplyDamage(damageValue);
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
        PlayerContex.RoamingPoint = targetPosition;
        this.targetPosition = targetPosition;
    }

    private float CheckDistanceToTarget()
    {
        if (targetPosition == null)
        {
            return 0;
        }
        
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

    private void HandleEating(IEatable eatable, FishType eatyingBy, int eaterFishIndex)
    {
        if (FoodIndex != eaterFishIndex)
        {
            Debug.Log($"EnemyFish - HandleEating] Index of this is {FoodIndex} is eaten by {eaterFishIndex}");
            return;
        }
        
        eatable.Eat(eatyingBy);
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
        
        PlayerContex playerContex = Contex as PlayerContex;

        if (condition)
        {
            //reset state machine
            _stateMachine.ResetStateMachine();
            playerContex.IsIdle = false;
            playerContex.IsRoaming = false;
        }

        FishMouth.SetMouthState(condition);
        //Debug.Log($"[PlayerFish - SetActiveFish] Set FishMouth IsMouthOpen to {condition}");

        playerContex.IsActiveFish = condition;
        _isActiveFish = condition;
    }

    private void PlayerEating()
    {
        Debug.Log("[PlayerFish - PlayerEating] Player Fish is Eating!");
        FishSpeed.ResetStackChaseSpeed();
        _playerFishHunger.ResetHunggerBar();
    }
}
