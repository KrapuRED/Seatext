using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EnemyContex : FishContex
{
    public FishSpawnerType EnemyFishSpawnerType;
    public Transform enemyPosition;
    public Transform endWaypoint;
    public float chargeTimer;
    public Transform chargeDirection;
    
    public FishEyeSight enemyFishEyeSight;
    public FishMouth enemyFishMouth;
    public EnemyFishTypeBox enemyFishTypeBox;
    public FishSightVisual fishSightVisual;
    public EnemyFish enemyFish;
    public int foodIndex;
    public bool beenlocked;
}

public class EnemyFish : Fish, IPausable, IEatable
{
    [Header("Fish Config")]
    [SerializeField] private Transform          EndWayPoint;
    [SerializeField] private FishSightVisual    _fishSightVisual;
    [SerializeField] private EnemyFishTypeBox   _enemyFishTypeBox;
    [SerializeField] private FishTextRotate     _fishTextRotation;
    [SerializeField] private int foodBeenEaten;
    [SerializeField] private bool intilazeFishByStart;

    private Rigidbody2D _rb2d;
    private EnemyContex _enemyContex;
    
    public int FoodBeenEaten => foodBeenEaten;
    public EnemyContex enemyContex => _enemyContex;
    
    public FoodSize foodSize { get; set; }

    private void Start()
    {
        if (intilazeFishByStart)
            IntilazeFish(EndWayPoint, FishSpawnerType.None, FishData, FoodIndex);
    }

    private void OnEnable()
    {
        GameEvents.OnEatableEntered.AddListener(HandleEating);
        GameEvents.OnApplyingSkillEffect.AddListener(HandleSkillEffect);
        
    }

    private void OnDestroy()
    {
        OnRemoveListener();
    }

    private void OnRemoveListener()
    {
        GameEvents.OnEatableEntered.RemoveListener(HandleEating);
        PauseManager.instance.Unregister(this);
        
        GameEvents.OnApplyingSkillEffect.RemoveListener(HandleSkillEffect);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IEatable eatable =  collision.collider.GetComponent<IEatable>();
            
            OnEating(eatable);
        }
    }

    public void OnPause()
    {
        enabled = false;
        FishMovement.SetCanMove(false);
    }

    public void OnResume()
    {
        enabled = true;
        FishMovement.SetCanMove(true);
    }

    public void IntilazeFish(Transform endWayPoint, FishSpawnerType fishSpawnerType ,FishSO data, int foodIndex)
    {
        EndWayPoint = endWayPoint;

        _rb2d = GetComponent<Rigidbody2D>();
        SetFishData(data, foodIndex);

        Contex = new EnemyContex
        {
            EnemyFishSpawnerType =  fishSpawnerType,
            fishObject          = gameObject,
            enemyPosition       = transform,
            endWaypoint         = EndWayPoint,
            
            fishMovement        = FishMovement,
            enemyFishEyeSight   = FishEyeSight,
            fishMouth           = FishMouth,
            enemyFishTypeBox    = _enemyFishTypeBox,
            fishSightVisual     = _fishSightVisual,
            fishSpeed           = FishSpeed,
            foodIndex           = base.FoodIndex,
            enemyFish           = this    
        };

        if (FishData.fishBehavior != FishBehavior.Passive)
        {
            FishEyeSight.isCanSee = true;
        }

        if (FishMouth != null)
        {
            FishMouth.ownerFish = this;
            FishMouth.SetMouthState(true);
        }
        
        _enemyContex = Contex as EnemyContex;
        FishSpeed.ownerFishType = FishType;
        FishSpeed.InitiliazeFishSpeed(FishData.speedFish);
        
        _enemyFishTypeBox.setTypeBoxEvent.Raise(_enemyFishTypeBox);
        _enemyFishTypeBox.SetTextToType(_enemyFishTypeBox.currentTextToType);
        
        FishMovement.IntilizaFishMovement(_rb2d, FishData);
        foodSize = FishData.fishSize;

        PauseManager.instance.Register(this);
    }

    private void HandleEating(IEatable eatable, FishType eatyingBy, int eaterFishIndex)
    {
        Debug.Log($"[EnemyFish - HandleEating] HandleEating Been Called!");
        
        if (FoodIndex != eaterFishIndex)
        {
            return;
        }
        
        Debug.Log($"[EnemyFish - HandleEating] I am {FoodIndex}, I ate something.");
        if (eatyingBy != FishType.Player)
            foodBeenEaten++;
        
        eatable.Eat(eatyingBy);
    }

    public override void OnEating(IEatable eatable)
    {
        if (eatable == null)
        {
            Debug.LogWarning($"Eating is null! in {gameObject.name}");
            return;
        }
        
        if (FishData.fishSize == FoodSize.Big)
        {
            eatable.Eat(FishType.Big);
        }
    }

    public void Eat(FishType fishType)
    {
        if (FishData.fishSize == FoodSize.Big)
        {
            Debug.Log($"{gameObject.name} Eat {fishType}!");
            return;
        }
        
        if (fishType == FishType.Player)
        {
            switch (fishType)
            {
                case FishType.Player:
                    Debug.Log($"[IEatable EnemyFish - Eat] {gameObject.name} has been eaten by Player Fish!");
                    GameEvents.OnPlayerEating.Invoke();
                    break;

                case FishType.Small:
                    Debug.Log($"[IEatable EnemyFish - Eat] {gameObject.name} has been eaten by Small Fish!");
                    GameEvents.OnRemoveSpawnedFishData.Invoke(FishSpawnerType.Passive, FoodIndex);
                    break;
            }
        }

        _enemyFishTypeBox.RemoveWordFromFish();
        Destroy(gameObject);
    }

    public override void HandleSkillEffect(bool isSkillActive, 
        AreaSkillEffectType? areaSkillEffectType,
        FishSkillEffectType? fishSkillEffect,
        float effectValue)
    {
        if (isBeenEffected)
            return;

        if (isSkillActive == false)
        {
            isBeenEffected = false;
            return;
        }
        
        isBeenEffected = true; 

        if (fishSkillEffect == FishSkillEffectType.Movement)
            FishSpeed.ReduceFishSpeed(isSkillActive, effectValue);
    }
}
