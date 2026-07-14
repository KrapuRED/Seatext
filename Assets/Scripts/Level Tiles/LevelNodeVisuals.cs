using System;
using UnityEngine;

public class LevelNodeVisuals : MonoBehaviour
{

    [Header("Node State Visuals By Type")]
    [SerializeField] private Sprite normalNode;
    [SerializeField] private Sprite normalHardNode;
    [SerializeField] private Sprite treasureNode;
    [SerializeField] private Sprite turtelMasterNode;
    [SerializeField] private Sprite startPointNode;
    [SerializeField] private Sprite endPointNode;
    [SerializeField] private Sprite defaultNode;

    private SpriteRenderer _spriteRenderer;

    [Header("Node State Selected Visuals By Type")]
    [SerializeField] private GameObject selectedNodeVisual;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetVisualLevelNodeByState(LevelNodeState state)
    {
        _spriteRenderer.sprite = state switch
        {
            LevelNodeState.Current => startPointNode,
            LevelNodeState.Explored => defaultNode,
            _ => _spriteRenderer.sprite
        };
    }

    public void SetVisualLevelNodeByType(LevelNodeType type)
    {
        _spriteRenderer.sprite = type switch
        {
            LevelNodeType.Normal => defaultNode,
            LevelNodeType.StartPoint => startPointNode,
            LevelNodeType.EndPoint => endPointNode,
            LevelNodeType.Treasure => treasureNode,
            LevelNodeType.TurtelMasterNode => turtelMasterNode,
            _ => _spriteRenderer.sprite
        };
    }

    public void SetVisualLevelNodeByLevelDifficulty(LevelDifficulty difficulty, LevelNodeState state)
    {
        if (state == LevelNodeState.Unseen)
            return;
        
        _spriteRenderer.sprite = difficulty switch
        {
            LevelDifficulty.None => defaultNode,
            LevelDifficulty.Normal => normalNode,
            LevelDifficulty.Hard => normalHardNode,
            _ => defaultNode
        };
    }

    public void ActiveSelectNodeVisual() => selectedNodeVisual.SetActive(true);
    public void HideSelecttNodeVisual() => selectedNodeVisual.SetActive(false);
}
