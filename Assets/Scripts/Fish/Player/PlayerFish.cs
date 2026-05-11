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
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float maxHealth;
    [SerializeField] private bool _isActiveFish;

    [Header("Fish System")]
    [SerializeField] private FishHunger _playerFishHunger;
    [SerializeField] private FishHealth _playerFishHealth;
    [SerializeField] private StateMachine _stateMachine;

    private Rigidbody2D _rb2d;

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
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerActive.RemoveListener(SetActiveFish);
        GameEvents.OnDodgeAttackFish.RemoveListener(DodgeAttackFish);
        GameEvents.OnSetPositionPlayerEvent.AddListener(SetPlayerFishDirection);
        GameEvents.OnPlayerEating.RemoveListener(PlayerEating);
    }

    private void OnDestroy()
    {
        PauseManager.instance.Unregister(this);
    }

    private void Update()
    {
        _playerFishHunger.Starve();

        if (targetPosition == null)
            return;
        
        if (CheckDistanceToTarget() <= distanceToTarget && (!PlayerContex.IsIdle || !PlayerContex.IsRoaming))
        {
            targetPosition = null;
            return;
        }

        if (PlayerContex.IsRoaming)
        {
            //Debug.Log($"[PlayerFish - Update] Move PlayerFish to Roaming Point");
            FishMovement.MoveFish(targetPosition, CheckDistanceToTarget() ,FishSpeed.GetRoamingFishSpeed());
            return;
        }

        FishMovement.MoveFish(targetPosition, CheckDistanceToTarget() ,FishSpeed.GetChaseFishSpeed());
        FishAnimation.OnHandlingMovementAnimation(CheckDistanceToTarget());
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
        //Debug.Log("[PlayerFish - SetPlayerFishDirection] Try to Move PlayerFish");
        PlayerContex.RoamingPoint = targetPosition;
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
