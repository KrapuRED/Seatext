using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
    Explored
}

public class LevelNode : MonoBehaviour
{
    [FormerlySerializedAs("_tileType")]
    [Header("Level Node Config")]
    [SerializeField] private string _levelNodeID;
    [SerializeField] private LevelNodeType tileType;
    [SerializeField] private LevelDataSO _levelDataSO;
    [SerializeField] private LevelNodeState _levelNodeState;
    [SerializeField] private LevelNodeTypeBox _levelNodeTypeBox;
    [SerializeField] private LevelNodeTextUI _levelNodeTextUI;
    [SerializeField] private string panelID;

    [Header("Level Node Settings")]
    [SerializeField] private LayerMask _LevelNodeLayer;
    [SerializeField] private float _levelNodeRadius;
    [SerializeField] private Transform _levelNodeCheckPoint;

    
    public LevelNodeState LevelNodeState => _levelNodeState;
    public LevelNodeType TileType => tileType;
    public LevelDataSO LevelDataSO => _levelDataSO;
    public string LevelNodeID => _levelNodeID;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        GetCommpoentLevelNode();
        
        ResetToHidden();
    }

    void GetCommpoentLevelNode()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void IntiliazeLevelNode(string levelNodeID)
    { 
        _levelNodeID = levelNodeID;

        if (_spriteRenderer == null)
            GetCommpoentLevelNode();
        
        if (_spriteRenderer == null)
        {
            Debug.LogError($"[LevelNode] _spriteRenderer missing on {gameObject.name}");
            return;
        }
        
        if (GameStateManager.Instance.IsLevelNodeBeenExplored(_levelNodeID))
        {
            SetBeenExplored();
            return;
        }
        
        if (tileType == LevelNodeType.EndPoint)
            _spriteRenderer.color = Color.red;
        
        if (tileType == LevelNodeType.StartPoint)
        {
            _levelNodeState = LevelNodeState.Current;
            _levelNodeTextUI.SetWordTextUI("You");
            _spriteRenderer.color = Color.blue;

            CheckSurroundingLevelNode();
        }
    }

    public void CheckSurroundingLevelNode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _levelNodeRadius, _LevelNodeLayer);

        foreach (var hitCollider in hitColliders)
        {
            //Debug.Log("Found: " + hitCollider.name);
            if (hitCollider.TryGetComponent(out LevelNode levelNode)){
                if (levelNode.LevelNodeState == LevelNodeState.Unseen)
                {
                    //Debug.Log($"[{this.name} - CheckSurroundingLevelNode] Set Near Level Node : {levelNode.name}");
                    GameEvents.OnSetNearCurrentLevelNode.Invoke(levelNode);
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

        PanelManager.instance.OpenPanel(panelID, _levelDataSO);
        
        if (_levelNodeState != LevelNodeState.Explored)
            GameEvents.OnSelectedNextLevelNode.Invoke(this);
    }


    public void SetSaroundingTilesBeenSeen()
    {
        if (_levelNodeState == LevelNodeState.Explored)
        {
            return;
        }
        
        _levelNodeTypeBox.GetWord();
        _spriteRenderer.color = Color.yellow;
        _levelNodeTypeBox.setTypeBoxEvent.Raise(_levelNodeTypeBox);
    }   

    public void SetBeenExplored()
    {
        _levelNodeState = LevelNodeState.Explored;
        
        if (_spriteRenderer == null)
            GetCommpoentLevelNode();
        
        _spriteRenderer.color = Color.grey;
        _levelNodeTextUI.SetWordTextUI("");
        _levelNodeTextUI.HideText();
    }

    public void ResetToHidden()
    {
        _levelNodeState = LevelNodeState.Unseen;

        _levelNodeTypeBox.RemoveWordData();
        _levelNodeTypeBox.ResetTypeBox();

        //_spriteRenderer.color = Color.white;
        _levelNodeTextUI.HideText();
    }
}