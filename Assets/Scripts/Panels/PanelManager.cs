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
    public Panel panel;
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
       GameEvents.OnClosePanelByID.AddListener(ClosePanel);
    }

    private void OnDisable()
    {
        GameEvents.OnClosePanelByID.RemoveListener(ClosePanel);
    }
    
    private void OnDestroy()
    {
        GameEvents.OnClosePanelByID.RemoveListener(ClosePanel);
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
                panel = panel
            };
            
            panelDatas.Add(panelData);
        }
    }

    private void OpenPanelByTypePanel(PanelType panelType)
    {
        
    }

    public void OpenPanelByID(string panelID, object data = null)
    {
        foreach (var panelData in panelDatas)
        {
            if (panelData.panel == null)
                continue;
            
            if (panelData.panel.panelID == panelID && !panelData.isActive)
            {
                TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.UI);
                panelData.panel.SetDataToPanel(data);
                panelData.isActive = true;
                panelData.panel.OpenPanel();
                break;
            }
        }
    }

    public void ClosePanel(string panelID)
    {
        TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.GamePlay);
        
        foreach (var panelData in panelDatas)
        {
            if (panelData.panel == null)
                continue;
            
            if (panelData.panel.panelID == panelID && panelData.isActive)
            {
                //Debug.Log($"[PanelManager - ClosePanel] Close Panel : {panelData.panelName}");
                panelData.isActive = false;
                panelData.panel.ClosePanel();
                break;
            }
        }
    }

    public void CloseAllPanel()
    {
        TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.GamePlay);
        foreach (var panelData in panelDatas)
        {
            panelData.isActive = false;
            panelData.panel.ClosePanel();
        }
    }
}
