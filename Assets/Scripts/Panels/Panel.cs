using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    public string panelName;
    public string panelID;
    public GameObject panelGO;
    public PanelType panelType;

    public abstract void GetPanelComponents();
    
    public abstract void OpenPanel();

    public abstract void ClosePanel();

    public abstract void SetDataToPanel(object data);
}
