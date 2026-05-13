using UnityEngine;

public class LevelNodeVisuals : MonoBehaviour
{
    [Header("Node State Visuals")]
    [SerializeField] private Sprite _unseenSprite;
    [SerializeField] private Color _unseenColor = Color.white;
    [SerializeField] private Color _seenColor = Color.yellow;
    [SerializeField] private Color _currentColor = Color.blue;
    [SerializeField] private Color _exploredColor = Color.grey;
    [SerializeField] private Color _endPointColor = Color.red;

    private SpriteRenderer _spriteRenderer;

    public void InitializedLevelNodeVisuals(SpriteRenderer spriteRender)
    {
        _spriteRenderer = spriteRender;
    }

    public void SetVisualLevelNodeByState(LevelNodeState state)
    {
        _spriteRenderer.color = state switch
        {
            LevelNodeState.Unseen => _unseenColor,
            LevelNodeState.Seen => _seenColor,
            LevelNodeState.Current => _currentColor,
            LevelNodeState.Explored => _exploredColor,
            _ => _spriteRenderer.color
        };
    }

    public void SetVisualLevelNodeByType(LevelNodeType type)
    {
        _spriteRenderer.color = type switch
        {
            LevelNodeType.StartPoint => _currentColor,
            LevelNodeType.EndPoint => _endPointColor,
            _ => _spriteRenderer.color
        };
    }
}
