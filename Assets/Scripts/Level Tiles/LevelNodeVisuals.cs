using UnityEngine;

public class LevelNodeVisuals : MonoBehaviour
{

    [Header("Node State Visuals By Type")]
    [SerializeField] private Sprite _treasureNode;
    [SerializeField] private Sprite _turtelMasterNode;
    [SerializeField] private Sprite _startPointNode;
    [SerializeField] private Sprite _endPointNode;
    [SerializeField] private Sprite _defaultNode;

    private SpriteRenderer _spriteRenderer;

    [Header("Node State Selected Visuals By Type")]
    [SerializeField] private GameObject _selectedNodeVisual;

    public void InitializedLevelNodeVisuals(SpriteRenderer spriteRender)
    {
        _spriteRenderer = spriteRender;
    }

    public void SetVisualLevelNodeByState(LevelNodeState state)
    {
        _spriteRenderer.sprite = state switch
        {
            LevelNodeState.Current => _startPointNode,
            LevelNodeState.Explored => _startPointNode,
            _ => _spriteRenderer.sprite
        };
    }

    public void SetVisualLevelNodeByType(LevelNodeType type)
    {
        _spriteRenderer.sprite = type switch
        {
            LevelNodeType.Normal => _defaultNode,
            LevelNodeType.StartPoint => _startPointNode,
            LevelNodeType.EndPoint => _endPointNode,
            LevelNodeType.Treasure => _treasureNode,
            LevelNodeType.TurtelMasterNode => _turtelMasterNode,
            _ => _spriteRenderer.sprite
        };
    }

    public void ActiveSelectNodeVisual() => _selectedNodeVisual.SetActive(true);
    public void HideSelecttNodeVisual() => _selectedNodeVisual.SetActive(false);
}
