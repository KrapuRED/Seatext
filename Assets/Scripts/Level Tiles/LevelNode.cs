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

public enum LevelNodeState
{
    Unseen,
    Seen,
    Current,
    Visited
}

public class LevelNode : MonoBehaviour
{
    [Header("Level Tile Config")]
    [SerializeField] private LevelTileType _tileType;
    [SerializeField] private LevelSO _levelData;
    [SerializeField] private LevelNodeState _levelNodeState;
    [SerializeField] private LevelNodeTypeBox _levelNodeTypeBox;
    [SerializeField] private LevelNodeTextUI _levelNodeTextUI;

    [Header("Level Node ")]
    [SerializeField] private LayerMask _LevelNodeLayer;
    [SerializeField] private float _levelNodeRadius;
    [SerializeField] private Transform _levelNodeCheckPoint;

    public LevelNodeState levelNodeState => _levelNodeState;
    public LevelTileType tileType => _tileType;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ResetToHidden();
    }

    private void Start()
    {
        IntiliazeLevelNode();
    }

    private void IntiliazeLevelNode()
    {
        if (_tileType == LevelTileType.StartPoint)
        {
            _levelNodeState = LevelNodeState.Current;
            _levelNodeTextUI.SetWordTextUI("You");
            _spriteRenderer.color = Color.blue;
            CheckSurroundingLevelNode();
        }

        if (_tileType == LevelTileType.EndPoint)
            _spriteRenderer.color = Color.red;

        LevelNodeManager.instance.RegisterLevelNode(this);
    }

    public void CheckSurroundingLevelNode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _levelNodeRadius, _LevelNodeLayer);

        foreach (var hitCollider in hitColliders)
        {
            //Debug.Log("Found: " + hitCollider.name);
            if (hitCollider.TryGetComponent(out LevelNode levelNode)){
                if (levelNode.levelNodeState == LevelNodeState.Unseen)
                {
                    //Debug.Log($"[{this.name} - CheckSurroundingLevelNode] Set Near Level Node : {levelNode.name}");
                    LevelNodeManager.instance.SetNearCurrentLevelNode(levelNode);
                    levelNode.SetSaroundingTilesBeenSeen();
                }
            }
        }
    }

    public void SetPlayerHere()
    {
        _levelNodeState = LevelNodeState.Current;
        _levelNodeTextUI.SetWordTextUI("You");
        _spriteRenderer.color = Color.blue;

        
        LevelNodeManager.instance.SelectedNextLevelNode(this);
    }


    public void SetSaroundingTilesBeenSeen()
    {
        
        if (_levelNodeState != LevelNodeState.Visited && _levelNodeState != LevelNodeState.Current)
        {
            _levelNodeTypeBox.GetWord();
            _spriteRenderer.color = Color.yellow;
            _levelNodeTypeBox.setTypeBoxEvent.Raise(_levelNodeTypeBox);
            
        }
    }   

    public void SetBeenVisited()
    {
        _levelNodeState = LevelNodeState.Visited;
        _spriteRenderer.color = Color.grey;
        _levelNodeTextUI.SetWordTextUI("");
        _levelNodeTextUI.HideText();
    }

    public void ResetToHidden()
    {
        Debug.Log("[LevelNode - ResetToHidden] Reset Level Node : " + name);
        
        _levelNodeState = LevelNodeState.Unseen;

        _levelNodeTypeBox.RemoveWordData();
        _levelNodeTypeBox.ResetTypeBox();

        _spriteRenderer.color = Color.white;
        _levelNodeTextUI.HideText();
    }
}
