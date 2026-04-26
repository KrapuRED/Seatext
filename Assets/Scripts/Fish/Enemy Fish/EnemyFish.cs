using UnityEngine;

public class EnemyContex : FishContex
{
    public Transform enemyPosition;
    public Transform endWaypoint;

    public FishEyeSight enemyFishEyeSight;
    public FishMouth enemyFishMouth;
    public EnemyFishTypeBox enemyFishTypeBox;
    public FishSightVisual fishSightVisual;
    public EnemyFish enemyFish;
}

public class EnemyFish : Fish, IPausable, IEatable
{
    [Header("Fish Config")]
    [SerializeField] private Transform          EndWayPoint;
    [SerializeField] private FishSightVisual    _fishSightVisual;
    [SerializeField] private EnemyFishTypeBox   _enemyFishTypeBox;
    [SerializeField] private FishTextRotate     _fishTextRotation;
    [SerializeField] private int _foodBeenEaten;
    [SerializeField] private bool IntilazeFishByStart;

    private Rigidbody2D _rb2d;

    public int FoodBeenEaten => _foodBeenEaten;
    
    public FoodSize foodSize { get; set; }

    private void Start()
    {
        if (IntilazeFishByStart)
            IntilazeFish(EndWayPoint, FishData);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnEating();
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

    public void IntilazeFish(Transform endWayPoint, FishOS data)
    {
        GameEvents.OnEatableEntered.AddListener(HandleEating);

        EndWayPoint = endWayPoint;

        _rb2d = GetComponent<Rigidbody2D>();
        SetFishData(data);

        Contex = new EnemyContex
        {
            fishObject          = gameObject,
            enemyPosition       = transform,
            endWaypoint         = EndWayPoint,
            fishMovement        = FishMovement,
            enemyFishEyeSight   = FishEyeSight,
            fishMouth           = FishMouth,
            enemyFishTypeBox    = _enemyFishTypeBox,
            fishSightVisual     = _fishSightVisual,
            fishSpeed           = FishSpeed,
            enemyFish           = this    
        };

        if (FishData.fishBehavior != FishBehavior.Passive)
        {
            FishEyeSight.isCanSee = true;
        }

        if (FishMouth != null)
            FishMouth.ownerFishType = FishType;

        _enemyFishTypeBox.setTypeBoxEvent.Raise(_enemyFishTypeBox);
        _enemyFishTypeBox.SetTextToType(_enemyFishTypeBox.currentTextToType);
        FishMovement.IntilizaFishMovement(_rb2d, FishData);
        foodSize = FishData.fishSize;

        PauseManager.instance.Register(this);
    }

    private void HandleEating(IEatable eatable)
    {
        if (FishType == FishType.Tiny)
            return;

        _foodBeenEaten++;
        eatable.Eat(FishType);
    }

    public override void OnEating()
    {

    }

    public void Eat(FishType fishType)
    {
        Debug.Log($"[IEatable EnemyFish - Eat] {gameObject.name} has been eaten! by {fishType.ToString()}");
        
        switch (fishType)
        {
            case FishType.Player:
                Debug.Log($"[IEatable EnemyFish - Eat] {gameObject.name} has been eaten by Player Fish!");
                PlayerFish.playerFish.playerFishStatus.ResetHunggerBar(); 
                break;
        }

        _enemyFishTypeBox.RemoveWordFromFish();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameEvents.OnEatableEntered.RemoveListener(HandleEating);
        PauseManager.instance.Unregister(this);
    }
}
