using TMPro;
using UnityEngine;

public class AdditionalInformationPanel : Panel
{
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Additional Information Panel Config")]
    [SerializeField] private TMP_Text turtelMasterTitle;
    [SerializeField] private TMP_Text turtelMasterDescription;

    #region Event Subscriptions

    private void OnEnable()
    {
        GameEvents.OnShowAdditionalInformationPanel.AddListener(SetDataToPanel);
        GameEvents.OnHideAdditionalInformationPanel.AddListener(ClosePanel);
    }

    private void OnDisable()
    {   
        OnRemoveListener();
    }

    private void OnDestroy()
    {
        OnRemoveListener();
    }

    void OnRemoveListener()
    {
        GameEvents.OnShowAdditionalInformationPanel.RemoveListener(SetDataToPanel);
        GameEvents.OnHideAdditionalInformationPanel.RemoveListener(ClosePanel);

    }

    #endregion

    public override void GetPanelComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void OpenPanel()
    {
        if (this == null) return;

        if (canvasGroup == null)
        {
            GetPanelComponents();
        }

        canvasGroup.alpha = 1f;
    }

    public override void ClosePanel()
    {
        if (this == null) return;

        if (canvasGroup == null)
        {
            GetPanelComponents();
        }

        canvasGroup.alpha = 0f;
    }

    public override void SetDataToPanel(object data)
    {
        var perkData = data as TurtelMasterPerkSO;

        turtelMasterTitle.text = perkData.TurtelMasterPerkName;
        turtelMasterDescription.text = perkData.TurtelMasterPerkDescriptionr;

        OpenPanel();
    }
}
