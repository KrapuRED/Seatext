using UnityEngine;

public class SurvivePanel : Panel
{
    public CanvasGroup  canvasGroup;

    public override void OpenPanel()
    {
        canvasGroup.alpha = 1;
    }

    public override void ClosePanel()
    {
        canvasGroup.alpha = 0;
    }

    public override void SetDataToPanel(object data)
    {
        
    }
}
