using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PanelData 
{
    public string panelName;
    public string panelID;
    public bool isActive;
    public GameObject panelGO;
}

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;

    public List<PanelData> panelTypes = new List<PanelData>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenPanel(string panelID)
    {
        foreach (var panel in panelTypes)
        {
            if (panel.panelID == panelID && !panel.isActive)
            {
                Debug.Log($"[PanelManager - OpenPanel] Open Panel : {panel.panelName}");
                TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.UI);
                panel.isActive = true;
                panel.panelGO.SetActive(panel.isActive);
                break;
            }
        }
    }

    public void ClosePanel(string panelID)
    {
        foreach (var panel in panelTypes)
        {
            if (panel.panelID == panelID && panel.isActive)
            {
                Debug.Log($"[PanelManager - OpenPanel] Open Panel : {panel.panelName}");
                TypeBoxManager.instance.SetCurrentTypeMode(TypeTypingBox.GamePlay);
                panel.isActive = false;
                panel.panelGO.SetActive(panel.isActive);
                break;
            }
        }
    }
}
