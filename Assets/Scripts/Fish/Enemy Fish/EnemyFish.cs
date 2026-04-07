using UnityEngine;

public class EnemyContex
{
    public GameObject enemyObject;
    public Transform enemyPosition;
    public Transform endWypointPoint;

    public FishMovement enemyFishMovement;
    public FishEyeSight enemyFishEyeSight;
}

public class EnemyFish : Fish, IPausable, IEatable
{
    [Header("Fish Config")]
    [SerializeField] private Transform EndWayPoint;
    [SerializeField] private EnemyFishTypeBox _enemyFishTypeBox;

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

        Contex = new EnemyContex
        {
            enemyObject         = gameObject,
            enemyPosition       = transform,
            endWypointPoint     = EndWayPoint,
            enemyFishMovement   = fishMovement,
            enemyFishEyeSight   = fishEyeSight
        };

        _enemyFishTypeBox.setTypeBoxEvent.Raise(_enemyFishTypeBox);
        _enemyFishTypeBox.SetTextToType(_enemyFishTypeBox.currentTextToType);
        fishMovement.IntilizaFishMovement(GetComponent<Rigidbody2D>(), fishData);

        PauseManager.instance.Register(this);
    }

    public void Eat()
    {
        Debug.Log($"[PlayerFish - Eat] {gameObject.name} has been eaten!");
        gameObject.SetActive(false);
    }
}
