using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeInforamtionPanel : Panel
{
    [Header("Node Information Panel")]
    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private TextMeshProUGUI _levelDescriptionText;
    [SerializeField] private TMP_Text currentFlowText;
    [SerializeField] private Image _environmentImage;

    [SerializeField] private CanvasGroup _canvasGroup;
    
    private void OnDestroy()
    {
        Debug.Log($"[NodeInforamtionPanel] Destroyed: {gameObject.name}");
    }

    public override void GetPanelComponents()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void ClosePanel()
    {
        Debug.Log($"ClosePanelByID called on : {gameObject.name}", this);

        if (_canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup NULL -> Regetting");
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        if (_canvasGroup == null)
        {
            Debug.LogError("CanvasGroup STILL NULL");
            return;
        }
        
        _canvasGroup.alpha = 0;
    }

    public override void OpenPanel()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
        
        _canvasGroup.alpha = 1;
    }

    public override void SetDataToPanel(object data)
    {
        if (data is IPanelDisplayable displayableData)
        {
            _levelNameText.text = displayableData.DisplayName;

            if (_levelDescriptionText != null)
                _levelDescriptionText.text = displayableData.DisplayDescription;

            if (currentFlowText != null)
                currentFlowText.text = displayableData.DisplayFlow;

            if (_environmentImage != null)
                _environmentImage.sprite = displayableData.DisplaySprite;
        }
        else
        {
            Debug.LogError("Passed data does not implement IPanelDisplayable!");
        }
    }
}