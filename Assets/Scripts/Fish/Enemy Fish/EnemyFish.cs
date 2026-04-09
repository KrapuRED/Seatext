using UnityEngine;

public class EnemyContex
{
    public GameObject enemyObject;
    public Transform enemyPosition;
    public Transform endWypointPoint;

    public FishMovement enemyFishMovement;
    public FishEyeSight enemyFishEyeSight;
    public EnemyFishTypeBox enemyFishTypeBox;
    public EnemyFish enemyFish;
}

public class EnemyFish : Fish, IPausable, IEatable
{
    [Header("Fish Config")]
    [SerializeField] private Transform EndWayPoint;
    [SerializeField] private EnemyFishTypeBox _enemyFishTypeBox;
    [SerializeField] private FishTextRotate _fishTextRotation;

    private Rigidbody2D _rb2d;

    public EnemyContex Contex { get; private set; }
    public bool IsEdible { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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

    public void IntilazeFish(Transform endWayPoint)
    {
        Debug.Log($"[Fish - Start] Fish Name : {fishData.fishName}");
        EndWayPoint = endWayPoint;
        _rb2d = GetComponent<Rigidbody2D>();

        Contex = new EnemyContex
        {
            enemyObject         = gameObject,
            enemyPosition       = transform,
            endWypointPoint     = EndWayPoint,
            enemyFishMovement   = fishMovement,
            enemyFishEyeSight   = fishEyeSight,
            enemyFishTypeBox    = _enemyFishTypeBox,
            enemyFish           = this    
        };

        _enemyFishTypeBox.setTypeBoxEvent.Raise(_enemyFishTypeBox);
        _enemyFishTypeBox.SetTextToType(_enemyFishTypeBox.currentTextToType);
        fishMovement.IntilizaFishMovement(_rb2d, fishData);

        PauseManager.instance.Register(this);
    }

    public void Eat()
    {
        Debug.Log($"[PlayerFish - Eat] {gameObject.name} has been eaten!");
        gameObject.SetActive(false);
    }

    public void Unregister()
    {
        PauseManager.instance.Unregister(this);
    }
}
