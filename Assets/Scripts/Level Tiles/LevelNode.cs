using TMPro;
using UnityEngine;

public enum LevelNodeType
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
    [Header("Level Node Config")]
    [SerializeField] private LevelNodeType _tileType;
    [SerializeField] private LevelDataSO _levelData;
    [SerializeField] private LevelNodeState _levelNodeState;
    [SerializeField] private LevelNodeTypeBox _levelNodeTypeBox;
    [SerializeField] private LevelNodeTextUI _levelNodeTextUI;
    [SerializeField] private string panelID;

    [Header("Level Node Settings")]
    [SerializeField] private LayerMask _LevelNodeLayer;
    [SerializeField] private float _levelNodeRadius;
    [SerializeField] private Transform _levelNodeCheckPoint;

    
    public bool beenExplored { get; set; }
    public LevelNodeState levelNodeState => _levelNodeState;
    public LevelNodeType TileType => _tileType;
    public LevelDataSO levelData => _levelData;

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
        if (_tileType == LevelNodeType.StartPoint)
        {
            _levelNodeState = LevelNodeState.Current;
            _levelNodeTextUI.SetWordTextUI("You");
            _spriteRenderer.color = Color.blue;

            CheckSurroundingLevelNode();
        }

        if (_tileType == LevelNodeType.EndPoint)
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

        PanelManager.instance.OpenPanel(panelID, _levelData);
        
        if (beenExplored)
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
        _levelNodeState = LevelNodeState.Unseen;

        _levelNodeTypeBox.RemoveWordData();
        _levelNodeTypeBox.ResetTypeBox();

        _spriteRenderer.color = Color.white;
        _levelNodeTextUI.HideText();
    }
}
