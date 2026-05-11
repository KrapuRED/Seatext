using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeInforamtionPanel : Panel
{
    [Header("Node Information Panel")]
    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private TextMeshProUGUI _levelDescriptionText;
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
        Debug.Log($"ClosePanel called on : {gameObject.name}", this);

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
        LevelDataSO levelData = data as LevelDataSO;

        //Debug.Log($"[NodeInforamtionPanel - SetDataToPanel] Set Data To Panel : {levelData.levelName}");

        _levelNameText.text = levelData.levelName;
        _levelDescriptionText.text = levelData.levelDescription;

    }
}