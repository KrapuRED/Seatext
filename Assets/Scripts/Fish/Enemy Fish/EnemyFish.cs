using UnityEngine;

public class EnemyContex
{
    public GameObject enemyObject;
    public Transform enemyPosition;
    public Transform endWypointPoint;

    public FishMovement enemyFishMovement;
    public FishEyeSight enemyFishEyeSight;
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
    [SerializeField] private bool IntilazeFishByStart;

    private Rigidbody2D _rb2d;

    public EnemyContex Contex { get; private set; }
    public FoodSize foodSize { get; set; }

    public void OnPause()
    {
        enabled = false;
        fishMovement.SetCanMove(false);
    }

    public void OnResume()
    {
        enabled = true;
        fishMovement.SetCanMove(true);
    }

    private void Start()
    {
        if (IntilazeFishByStart)
            IntilazeFish(EndWayPoint, fishData);
    }

    public void IntilazeFish(Transform endWayPoint, FishOS data)
    {
        EndWayPoint = endWayPoint;

        _rb2d = GetComponent<Rigidbody2D>();
        SetFishData(data);

        Debug.Log($"[Fish - Start] Fish Name : {fishData.fishName}");

        Contex = new EnemyContex
        {
            enemyObject         = gameObject,
            enemyPosition       = transform,
            endWypointPoint     = EndWayPoint,
            enemyFishMovement   = fishMovement,
            enemyFishEyeSight   = fishEyeSight,
            enemyFishTypeBox    = _enemyFishTypeBox,
            fishSightVisual     = _fishSightVisual,
            enemyFish           = this    
        };

        if (fishData.fishBehavior != FishBehavior.Passive)
        {
            fishEyeSight.isCanSee = true;
        }

        _enemyFishTypeBox.setTypeBoxEvent.Raise(_enemyFishTypeBox);
        _enemyFishTypeBox.SetTextToType(_enemyFishTypeBox.currentTextToType);
        fishMovement.IntilizaFishMovement(_rb2d, fishData);
        foodSize = fishData.fishSize;

        PauseManager.instance.Register(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEatable eatable = collision.GetComponent<IEatable>();

        if (eatable == null)
            return;

        eatable.Eat(fishType);
    }

    public void Eat(FishType fishType)
    {
        Debug.Log($"[PlayerFish - Eat] {gameObject.name} has been eaten!");
        _enemyFishTypeBox.RemoveWordFromFish();
        
        switch (fishType)
        {
            case FishType.Player:
                PlayerFish.playerFish.ResetHunggerBar(); 
                break;
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PauseManager.instance.Unregister(this);
    }
}
