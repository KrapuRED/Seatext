using UnityEngine;
using System.Collections.Generic;

public enum PanelType
{
    PanelPause,
    PanelSurvive,
    NodePanelNormal,
    NodePanelTreasure,
    NodePanelTurtelMaster
}

[System.Serializable]
public class PanelData 
{
    public string panelName;
    public PanelType panelType;
    public bool isActive;
    public Panel Panel;
}

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;

    [SerializeField] private Transform _panelContainer;
    public List<PanelData> panelDatas = new List<PanelData>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    
        instance = this;
        
        panelDatas.Clear();
        OnRegisterPanel();
    }
    
    private void OnEnable()
    {
       GameEvents.OnClosePanelByID.AddListener(ClosePanelByID);
    }

    private void OnDisable()
    {
        GameEvents.OnClosePanelByID.RemoveListener(ClosePanelByID);
    }
    
    private void OnDestroy()
    {
        GameEvents.OnClosePanelByID.RemoveListener(ClosePanelByID);
    }

    public void OnRegisterPanel()
    {
        foreach (Transform panelObject in _panelContainer)
        {
            Panel panel = panelObject.GetComponentInChildren<Panel>();
            
            if (panel == null)
                continue;
            
            panel.GetPanelComponents();
            
            PanelData panelData = new PanelData
            {
                panelName =  panel.name,
                isActive =  false,
                panelType = panel.panelType,
                Panel = panel
            };
            
            panelDatas.Add(panelData);
        }
    }

    public void OpenPanelByTypePanel(PanelType panelType, object data = null)
    {
        var panelData = panelDatas.Find(panelData => panelData.panelType == panelType);

        Debug.Log($"[PanelManager - OpenPanelByTypePanel] Open Panel : {panelData.Panel.name} with type {panelType}");

        if (panelData == null)
        {
            Debug.LogError($"[PanelManager - OpenPanelByTypePanel] Panel with type {panelType} not found!");
            return;
        }

        if (data != null)
        {
            panelData.Panel.SetDataToPanel(data);
        }

        TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.UI);

        panelData.isActive = true;
        panelData.Panel.OpenPanel();
    }

    public void OpenPanelByID(string panelID, object data = null)
    {
        foreach (var panelData in panelDatas)
        {
            if (panelData.Panel == null)
                continue;
            
            if (panelData.Panel.panelID == panelID && !panelData.isActive)
            {
                TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.UI);
                panelData.Panel.SetDataToPanel(data);
                panelData.isActive = true;
                panelData.Panel.OpenPanel();
                break;
            }
        }
    }

    public void ClosePanelByID(string panelID)
    {
        TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.GamePlay);
        
        foreach (var panelData in panelDatas)
        {
            if (panelData.Panel == null)
                continue;
            
            if (panelData.Panel.panelID == panelID && panelData.isActive)
            {
                //Debug.Log($"[PanelManager - ClosePanelByID] Close Panel : {panelData.panelName}");
                panelData.isActive = false;
                panelData.Panel.ClosePanel();
                break;
            }
        }
    }

    public void ClosePanelByPanelType(PanelType panelType)
    {
        var panelData = panelDatas.Find(panelData => panelData.panelType == panelType);

        if (!panelData.isActive)
        {
            Debug.LogWarning($"[PanelManager - ClosePanelByPanelType] Panel with type {panelType} is not active!");
            return;
        }

        TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.GamePlay);
        panelData.Panel.ClosePanel();
        panelData.isActive = false;
    }

    public void CloseAllPanel()
    {
        TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.GamePlay);
        foreach (var panelData in panelDatas)
        {
            panelData.isActive = false;
            panelData.Panel.ClosePanel();
        }
    }
}
