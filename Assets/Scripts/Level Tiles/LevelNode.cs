using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public enum LevelNodeType
{
    Normal,
    Treasure,
    TurtelMasterNode,
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
    [SerializeField] private LevelNodeVisuals _levelNodeVisuals;

    [Header("Panel Level Node Config")]
    [SerializeField] private PanelType panelType; 
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
    
    private bool _isBeenInitialized = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _levelNodeVisuals.InitializedLevelNodeVisuals(_spriteRenderer);
        ResetToHidden();
    }


    public void IntiliazeLevelNode(string levelNodeID)
    { 
        _levelNodeID = levelNodeID;
        _isBeenInitialized = true;
        
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

        if (LevelNodeType.StartPoint == tileType)
        {
            OnSetPlayerHere();
        }
        else
        {
            _levelNodeVisuals.SetVisualLevelNodeByType(tileType);
        }
    }

    public void CheckSurroundingLevelNode()
    {
        if (!_isBeenInitialized)
            return;
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _levelNodeRadius, _LevelNodeLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out LevelNode levelNode)){
                if (levelNode.LevelNodeState == LevelNodeState.Unseen || levelNode.LevelNodeState == LevelNodeState.Explored)
                {
                    GameEvents.OnSetNearCurrentLevelNode.Invoke(levelNode);
                    levelNode.SetSaroundingTilesBeenSeen();
                }
            }
        }
    }

    public void OnSetPlayerHere()
    {
        Debug.Log($"{gameObject.name} is Player Here!");
        _levelNodeState = LevelNodeState.Current;
        _levelNodeTextUI.SetWordTextUI("You");
        _levelNodeVisuals.SetVisualLevelNodeByType(tileType);

        GameEvents.OnChangeCameraPosition.Invoke(transform);

        CheckSurroundingLevelNode();
    }

    public void SelectedLevelNode()
    {
        _levelNodeState = LevelNodeState.Current;
        _levelNodeTextUI.SetWordTextUI("You");
        _levelNodeVisuals.SetVisualLevelNodeByState(_levelNodeState);

        GameEvents.OnSetLevelNode.Invoke(this);

        PanelManager.instance.OpenPanelByTypePanel(panelType, _levelDataSO);
        
        if (_levelNodeState != LevelNodeState.Explored)
            GameEvents.OnSelectedNextLevelNode.Invoke(this);
    }

    public void SetSaroundingTilesBeenSeen()
    {
        if (GameStateManager.Instance.IsLevelNodeBeenExplored(_levelNodeID))
        {
            SetBeenExplored();
            return;
        }

        _levelNodeState = LevelNodeState.Seen;

        if (tileType != LevelNodeType.Normal)
        {
            _levelNodeVisuals.SetVisualLevelNodeByType(tileType);
        }
        else
            _levelNodeVisuals.SetVisualLevelNodeByState(_levelNodeState);

        _levelNodeTypeBox.GetWord();
        _levelNodeTypeBox.setTypeBoxEvent.Raise(_levelNodeTypeBox);
    }   

    public void SetBeenExplored()
    {
        _levelNodeState = LevelNodeState.Explored;

        _levelNodeVisuals.SetVisualLevelNodeByState(_levelNodeState);
        _levelNodeTextUI.SetWordTextUI(string.Empty);
        _levelNodeTextUI.HideText();
        
        
    }

    public void ResetToHidden()
    {
        if (_levelNodeState == LevelNodeState.Explored)
            return;
        
        _levelNodeState = LevelNodeState.Unseen;
        _levelNodeTypeBox.RemoveWordData();
        _levelNodeTypeBox.ResetTypeBox();
        
        _levelNodeTextUI.HideText();
    }
}