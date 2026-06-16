using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public enum TurtleMasterPerk
{
    Ignore,
    Heal,
    RemoveTrash,
    Learn
}

public class TurtleMasterManager : MonoBehaviour
{
    public static TurtleMasterManager Instance { get; private set; }

    [SerializeField] private List<ButtonTypeBox> TurtleMasterButtons = new List<ButtonTypeBox>();

    [SerializeField] private LevelNode _levelNode;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #region Event Subscription

    private void OnEnable()
    {
        GameEvents.OnSetLevelNode.AddListener(OnSelectedNodeLevel);

        GameEvents.OnSingleTypeBoxMatch.AddListener(HandleSingleMatch);
        GameEvents.OnRemoveAllLevelNodeReferences.AddListener(OnRemovedListener);

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
        GameEvents.OnSetLevelNode.RemoveListener(OnSelectedNodeLevel);

        GameEvents.OnSingleTypeBoxMatch.RemoveListener(HandleSingleMatch);
        GameEvents.OnRemoveAllLevelNodeReferences.RemoveListener(OnRemovedListener);

    }

    #endregion

    private void HandleSingleMatch(object arg)
    {
        if (this == null) return;

        TypingBox typingBox = arg as TypingBox;

        if (typingBox == null)
        {
            Debug.Log($"[Turtel Master Manager] Type Box is Null, Please Check the Config of the Button");
            return;
        }

        TurtleMasterButtonUI turtleMasterButtonUI = typingBox.GetComponent<TurtleMasterButtonUI>();
        if (turtleMasterButtonUI == null)
        {
            Debug.Log($"[Turtel Master Manager] TurtleMasterButtonUI Component is Missing, Please Check the Config of the Button");
            return;
        }

        ShowAddtionalInfomation(turtleMasterButtonUI.PerkData);
    }

    private void OnSelectedNodeLevel(LevelNode levelNode)
    {
        if (this == null) return; // safety check in case event fires after object is destroyed

        _levelNode = levelNode;
    }

    private void CloseTurtelMasterButtonUI()
    {
        _levelNode = null; // clear ref so stale data can't leak between opens
        PanelManager.instance.ClosePanelByPanelType(PanelType.NodePanelTurtelMaster);
    }

    public void ApplyeTurtleMaster(TurtelMasterPerkSO perkData)
    {
        Debug.Log($"[Turtel Master Manager] Applying effect of {perkData.TurtelMasterPerk} with cost {perkData.costValue}");

        if (!CurrencyManager.instance.IsSufficientCurrecny(CurrencyType.Seacoene, perkData.costValue))
        {
            // Show information IsSufficientCurrecny
            Debug.LogWarning($"Unsufficient Currecny of {CurrencyType.Seacoene}");
            return;
        }

        switch (perkData.TurtelMasterPerk)
        {
            case TurtleMasterPerk.Heal:
                // Heal the player
                Debug.Log($"[Turlet Master Manager] Is Healing Player Fish with {perkData.percentageValue}%");
                StatusPlayerManager.Instance.HealingHealth(perkData.percentageValue);
                break;

            case TurtleMasterPerk.RemoveTrash:
                // Remove Trash from the player
                Debug.Log($"[Turlet Master Manager] Is Removing Trash Player Fish with cost {perkData.costValue}");
                StatusPlayerManager.Instance.CleanTrash();
                break;

            case TurtleMasterPerk.Learn:
                // Heal the player
                Debug.Log($"[Turlet Master Manager] Player Fish is Learming get ");
                break;

            default:
                Debug.Log($"[Turtel Master Manager] This is a Ignore, No Effect to Apply");
                break;
        }

        CurrencyManager.instance.UseCurrency(CurrencyType.Seacoene, perkData.costValue);
        GameEvents.OnSetLevelNodeBeenExplored.Invoke(_levelNode.LevelNodeID);
        CloseTurtelMasterButtonUI();
    }

    public void ShowAddtionalInfomation(TurtelMasterPerkSO perkData)
    {
        Debug.Log($"[Turtel Master Manager] Showing Additional Information for {perkData}");

        if (perkData.TurtelMasterPerk == TurtleMasterPerk.Ignore)
        {
            Debug.Log($"[Turtel Master Manager] This is a Ignore, No Additional Information to Show");
            HideAddtionalInfomation();
            return;
        }

        GameEvents.OnShowAdditionalInformationPanel.Invoke(perkData);
        Debug.Log($"[Turtel Master Manager] Showing Additional Information for {perkData.TurtelMasterPerk}");
    }

    public void HideAddtionalInfomation()
    {
        Debug.Log($"[Turtel Master Manager] Hiding Additional Information");
        GameEvents.OnHideAdditionalInformationPanel.Invoke();
    }
}
