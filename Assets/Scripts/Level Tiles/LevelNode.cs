using TMPro;
using UnityEngine;

public enum LevelTileType
{
    Normal,
    Treasure,
    TurtleMaster,
    StartPoint,
    EndPoint
}

public class LevelNode : MonoBehaviour
{
    [Header("Level Tile Config")]
    [SerializeField] private LevelTileType _tileType;
    [SerializeField] private LevelSO _levelData;
    [SerializeField] private LevelNodeTypeBox _levelNodeTypeBox;
    [SerializeField] private LevelNodeTextUI _levelNodeTextUI;

    [Header("Level Node ")]
    [SerializeField] private LayerMask _LevelNodeLayer;
    [SerializeField] private float _levelNodeRadius;
    [SerializeField] private Transform _levelNodeCheckPoint;

    public bool isPlayerhere { get; private set; }
    [SerializeField] private bool _isBeenVisited;
    [SerializeField] private bool _isBeenSeen;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        IntiliazeLevelNode();
    }

    private void IntiliazeLevelNode()
    {
        if (_tileType != LevelTileType.StartPoint)
        {
            //_levelNodeTextUI.SetWordTextUI(string.Empty);
        }
        else
        {
            isPlayerhere = true;
            _spriteRenderer.color = Color.blue;
            CheckSurroundingLevelNode();
        }

        if (_tileType == LevelTileType.EndPoint)
            _spriteRenderer.color = Color.red;

        LevelNodeManager.instance.RegisterLevelNode(this);
    }

    private void CheckSurroundingLevelNode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _levelNodeRadius, _LevelNodeLayer);

        foreach (var hitCollider in hitColliders)
        {
            Debug.Log("Found: " + hitCollider.name);
            if (hitCollider.TryGetComponent(out LevelNode levelNode)){
                if (!levelNode.isPlayerhere)
                {
                    levelNode.SetSaroundingTilesBeenSeen();
                }
            }
        }
    }

    public void SetSaroundingTilesBeenSeen()
    {
        _levelNodeTypeBox.GetWord();
        _spriteRenderer.color = Color.yellow;
    }   

    public void SetBeenVisited()
    {

    }


}
