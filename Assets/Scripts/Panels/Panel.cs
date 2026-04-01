using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    public string panelName;
    public string panelID;
    public GameObject panelGO;

    public abstract void OpenPanel();

    public abstract void ClosePanel();
}
