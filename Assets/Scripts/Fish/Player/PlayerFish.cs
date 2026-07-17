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
    public bool IsBerserk;
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
    [SerializeField] private bool isImmune;
    private float currentInvisible;

    [Header("Fish System")]
    [SerializeField] private FishHunger _playerFishHunger;
    [SerializeField] private FishHealth _playerFishHealth;
    [SerializeField] private StateMachine _stateMachine;

    private Rigidbody2D _rb2d;
    private Transform _moveTarget;
    private float _moveSpeed;
    
    private int foodEatenDuringBerserk;
    [SerializeField] private bool isBerserk;

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
        GameEvents.OnApplyingSkillEffect.AddListener(HandleSkillEffect);
    }

    private void OnDisable()
    {
        OnRemoveListener();
    }

    private void OnDestroy()
    {
       OnRemoveListener();
    }

    private void OnRemoveListener()
    {
        PauseManager.instance.Unregister(this);
        GameEvents.OnPlayerActive.RemoveListener(SetActiveFish);

        GameEvents.OnSetPositionPlayerEvent.AddListener(SetPlayerFishDirection);
        GameEvents.OnPlayerEating.RemoveListener(PlayerEating);
        
        GameEvents.OnEatableEntered.RemoveListener(HandleEating);
        
        GameEvents.OnApplyingSkillEffect.RemoveListener(HandleSkillEffect);
    }
    
    private void Update()
    {
        if (GameManager.instance.LevelDone)
            return;
        
        if (isImmune && currentInvisible > 0)
        {
            currentInvisible -= Time.deltaTime;
        }
        else
        {
            isImmune = false;
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
        if (isImmune)
            return;

        isImmune = true;
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

    public void SetTargetPosition(Transform targetFoodPosition)
    {
        targetPosition = targetFoodPosition;
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
        if (isBerserk)
            return;
        
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

    private void StartBerserk()
    {
        //Change the behavior of the player fish and boost 100% movement
        isBerserk = true;
        isImmune = true;
        foodEatenDuringBerserk = 0;
        
        FishSpeed.IncreaseFishSpeed(isBerserk, 100f);
        
        PlayerContex.IsRoaming = false;
        PlayerContex.IsIdle = false;
        PlayerContex.IsBerserk = true;
        
        Debug.Log("[PlayerFish] Berserk started - immune, auto-chasing food");
    }

    private void EndBerserk()
    {
        Debug.Log("[PlayerFish] Berserk ended");
        
        isBerserk = false;
        isImmune = false;
        PlayerContex.IsBerserk = false;
        
        FishSpeed.IncreaseFishSpeed(isBerserk, 0);
        
        var skillData = FishSkillManager.Instance.UseFishSkillData; // however you expose it
        if (foodEatenDuringBerserk < skillData.effectRequired)
        {
            float hpLoss = StatusPlayerManager.Instance.MaxPlayerHealthStatus * (skillData.effectValue / 100f);
            TakingDamage(hpLoss);
            Debug.Log($"[PlayerFish] Berserk ended - only ate {foodEatenDuringBerserk}, losing {skillData.effectValue}% HP");
        }
        
        _stateMachine.ResetStateMachine();
    }
    
    public override void HandleSkillEffect(bool isSkillActive, AreaSkillEffectType? areaSkillEffectType,
        FishSkillEffectType? fishSkillEffect,
        float effectValue)
    {
        if (!isSkillActive)
        {
            Debug.Log("Try to Inactive skill effect");
            
            isBeenEffected = false;

            if (fishSkillEffect == FishSkillEffectType.Movement)
                FishSpeed.IncreaseFishSpeed(isSkillActive, effectValue);

            EndBerserk();
            
            return;
        }
        
        if (isBeenEffected)
            return;
        
        Debug.Log("[PlayerFish - HandleSkillEffect] PlayerFish HandleSkillEffect");
        if (areaSkillEffectType == AreaSkillEffectType.Around)
        {
            Debug.Log("[PlayerFish - HandleSkillEffect] PlayerFish HandleSkillEffect not effected");
            return;
        }
        
        isBeenEffected = true;

        switch (fishSkillEffect)
        {
            case FishSkillEffectType.Movement:
                FishSpeed.IncreaseFishSpeed(isSkillActive, effectValue);
                break;
            case FishSkillEffectType.Berserk:
                StartBerserk();
                break;
            
        }
        
    }
}
