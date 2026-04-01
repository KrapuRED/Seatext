using UnityEngine;

public class PausePanel : Panel
{
    public CanvasGroup cg;

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
}
