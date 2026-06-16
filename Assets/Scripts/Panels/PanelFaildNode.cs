using UnityEngine;

public class PanelFaildNode : Panel
{
    private CanvasGroup canvasGroup; 

    public override void ClosePanel()
    {
        if (this == null) return;

        if (canvasGroup == null) GetPanelComponents();

        canvasGroup.alpha = 0;
    }

    public override void GetPanelComponents()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void OpenPanel()
    {
        if (this == null) return;

        if (canvasGroup == null) GetPanelComponents();

        canvasGroup.alpha = 1f;
    }

    public override void SetDataToPanel(object data)
    {
        throw new System.NotImplementedException();
    }
}
