using UnityEngine;

public class TreasureButtonUI : MonoBehaviour
{
    [SerializeField] private LevelNode _levelNode;

    private void OnEnable()
    {
        GameEvents.OnSetLevelNodeTreasure.AddListener(Initialize);
    }

    private void OnDisable()
    {
       OnRemovedListener();
    }

    private void OnDestroy()
    {
        OnRemovedListener();
    }

    private void OnRemovedListener()
    {
        GameEvents.OnSetLevelNodeTreasure.RemoveListener(Initialize);
    }

    public void Initialize(LevelNode levelNode)
    {
        if (this == null) return; // safety check in case event fires after object is destroyed

        _levelNode = levelNode;
    }

    public void OnCollectButtonClick()
    {
        Debug.Log("Collect button clicked!");

        if (_levelNode != null)
        {
            // Marks the node as explored, moves the player here, and persists
            // to GameStateManager — all handled inside LevelNodeManager.SetLevelNodeBeenExplored
            GameEvents.OnSetLevelNodeBeenExplored.Invoke(_levelNode.LevelNodeID);
            GameEvents.OnGetRandomTreasureItem.Invoke();

            CloseTreasureButtonUI();

        }
        else
        {
            Debug.LogWarning("[TreasureButtonUI] _levelNode is null — call Initialize() before opening the panel.");
        }
    }

    public void OnIgnoreButtonClick()
    {
        Debug.Log("Ignore button clicked!");
        // Player stays on the previous node — no state change needed
        CloseTreasureButtonUI();
    }

    private void CloseTreasureButtonUI()
    {
        _levelNode = null; // clear ref so stale data can't leak between opens
        PanelManager.instance.ClosePanelByPanelType(PanelType.NodePanelTreasure);
    }
}
