using UnityEngine;

public class PausePanel : Panel
{
    public CanvasGroup cg;

    private void Start()
    {
        if (cg.alpha == 1)
        {
            PanelManager.instance.OpenPanel(panelID);
        }
    }

    public override void OpenPanel()
    {
        PauseManager.instance.SetPause(true);
        cg.alpha = 1;
    }

    public override void ClosePanel()
    {
        PauseManager.instance.SetPause(false);
        cg.alpha = 0;
    }

    public override void SetDataToPanel(object data)
    {
        
    }
}
