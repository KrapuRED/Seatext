using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PanelData 
{
    public string panelName;
    public bool isActive;
    public Panel panel;
}

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;

    public List<PanelData> panelDatas = new List<PanelData>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenPanel(string panelID)
    {
        Debug.Log($"[PanelManager - OpenPanel] try opening panel : {panelID}");
        foreach (var panelData in panelDatas)
        {
            if (panelData.panel.panelID == panelID && !panelData.isActive)
            {
                Debug.Log($"[PanelManager - OpenPanel] Open Panel : {panelData.panelName}");
                TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.UI);
                panelData.isActive = true;
                panelData.panel.OpenPanel();
                break;
            }
        }
    }

    public void ClosePanel(string panelID)
    {
        foreach (var panelData in panelDatas)
        {
            if (panelData.panel.panelID == panelID && panelData.isActive)
            {
                Debug.Log($"[PanelManager - OpenPanel] Open Panel : {panelData.panelName}");
                TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.GamePlay);
                panelData.isActive = false;
                panelData.panel.ClosePanel();
                break;
            }
        }
    }
}
