using UnityEngine;

public class EnemyContex
{
    public GameObject enemyObject;
    public Transform enemyPosition;
    public Transform endWypointPoint;

    public FishMovement enemyFishMovement;
}

public class EnemyFish : Fish
{
    [Header("Fish Config")]
    [SerializeField] private Transform EndWayPoint;
    [SerializeField] private EnemyFishTypeBox _enemyFishTypeBox;

    public EnemyContex Contex { get; private set; }

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
    }
}
