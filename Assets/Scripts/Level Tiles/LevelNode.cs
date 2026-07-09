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
    [SerializeField] private LevelDataSO levelDataSO;
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
    public LevelDataSO LevelDataSO => levelDataSO;
    public string LevelNodeID => _levelNodeID;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private bool _isBeenInitialized = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _levelNodeVisuals.InitializedLevelNodeVisuals(_spriteRenderer);
        ResetToHidden();
    }


    public void IntiliazeLevelNode(string levelNodeID, LevelDataSO levelData)
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
            levelDataSO = levelData;

            _levelNodeVisuals.SetVisualLevelNodeByType(tileType);
        }
    }

    public void CheckSurroundingLevelNode()
    {
        if (GameManager.instance.IsFailed)
        {
            return;
        }
            
        if (!_isBeenInitialized)
            return;
    
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _levelNodeRadius, _LevelNodeLayer);

        float detectionAngle = 150f; // 90 (half-circle) + 60 (extra into the rear)

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out LevelNode levelNode))
            {
                Vector2 directionToNode = (levelNode.transform.position - transform.position).normalized;

                // Angle between forward direction and the node, 0-180
                float angle = Vector2.Angle(transform.right, directionToNode);

                if (angle > detectionAngle)
                    continue; // outside the widened cone, skip it

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
        _levelNodeState = LevelNodeState.Current;
        _levelNodeTextUI.SetWordTextUI("You");

        _levelNodeVisuals.SetVisualLevelNodeByState(_levelNodeState);
        _levelNodeVisuals.HideSelecttNodeVisual();

        GameEvents.OnChangeCameraPosition.Invoke(transform);

        CheckSurroundingLevelNode();
    }

    private PanelType CheckPanelTypeByTileType(LevelNodeType NodeType)
    {
        PanelType selectedPanelType = tileType switch
        {
            LevelNodeType.Normal           => PanelType.NodePanelNormal,
            LevelNodeType.Treasure         => PanelType.NodePanelTreasure,
            LevelNodeType.TurtelMasterNode => PanelType.NodePanelTurtelMaster,
            _                              => PanelType.None
        };
        panelType = selectedPanelType;
        return selectedPanelType;
    }
    
    public void SelectedLevelNode()
    {
        _levelNodeState = LevelNodeState.Current;
        _levelNodeTextUI.SetWordTextUI("You");
        _levelNodeVisuals.SetVisualLevelNodeByState(_levelNodeState);

        GameEvents.OnSetLevelNode.Invoke(this);

        PanelManager.instance.OpenPanelByTypePanel(CheckPanelTypeByTileType(tileType), levelDataSO);
        
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

        if (tileType != LevelNodeType.Normal && _levelNodeState != LevelNodeState.Explored)
        {
            _levelNodeVisuals.SetVisualLevelNodeByType(tileType);
        }
        else
            _levelNodeVisuals.SetVisualLevelNodeByState(_levelNodeState);

        _levelNodeVisuals.ActiveSelectNodeVisual();

        _levelNodeTypeBox.GetWord();
        _levelNodeTypeBox.setTypeBoxEvent.Raise(_levelNodeTypeBox);
    }   

    public void SetBeenExplored()
    {
        _levelNodeState = LevelNodeState.Explored;

        // Mirror the same pattern from SetSaroundingTilesBeenSeen
        if (tileType != LevelNodeType.Normal && _levelNodeState != LevelNodeState.Explored)
            _levelNodeVisuals.SetVisualLevelNodeByType(tileType);
        else
            _levelNodeVisuals.SetVisualLevelNodeByState(_levelNodeState);

        _levelNodeVisuals.HideSelecttNodeVisual();
        _levelNodeTextUI.SetWordTextUI(string.Empty);
        _levelNodeTextUI.HideText();
    }

    public void ResetToHidden()
    {
        if (_levelNodeState == LevelNodeState.Explored)
            return;

        _levelNodeVisuals.HideSelecttNodeVisual();
        _levelNodeVisuals.SetVisualLevelNodeByType(tileType);

        _levelNodeState = LevelNodeState.Unseen;
        _levelNodeTypeBox.RemoveWordData();
        _levelNodeTypeBox.ResetTypeBox();
        
        _levelNodeTextUI.HideText();
    }
}