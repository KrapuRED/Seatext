using UnityEngine;

public class EnemyContex
{
    public GameObject enemyObject;
    public Transform enemyPosition;
    public Transform endWypointPoint;

    public FishMovement enemyFishMovement;
}

public class EnemyFish : Fish, IPausable
{
    [Header("Fish Config")]
    [SerializeField] private Transform EndWayPoint;
    [SerializeField] private EnemyFishTypeBox _enemyFishTypeBox;

    public EnemyContex Contex { get; private set; }

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

    private void Awake()
    {
        IntilazeFish();
    }

    private void IntilazeFish()
    {
        Debug.Log($"[Fish - Start] Fish Name : {fishData.fishName}");

        Contex = new EnemyContex
        {
            enemyObject = gameObject,
            enemyPosition = transform,
            endWypointPoint = EndWayPoint,
            enemyFishMovement = fishMovement
        };

        _enemyFishTypeBox.setTypeBoxEvent.Raise(_enemyFishTypeBox);
        _enemyFishTypeBox.SetTextToType(_enemyFishTypeBox.currentTextToType);
        fishMovement.IntilizaFishMovement(GetComponent<Rigidbody2D>(), fishData);

        PauseManager.instance.Register(this);
    }
}
