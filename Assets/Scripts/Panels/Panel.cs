using UnityEngine;

public abstract class PanelBase : MonoBehaviour
{
    public abstract void GetPanelComponents();
    public abstract void OpenPanel();
    public abstract void ClosePanel();
    public abstract void SetDataToPanel(object data);
}

public abstract class Panel : PanelBase
{
    public string panelName;
    public string panelID;
    public GameObject panelGO;
    public PanelType panelType;
}
