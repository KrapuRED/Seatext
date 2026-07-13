using UnityEngine;

public class PanelPowerUps : Panel
{
    
    private CanvasGroup  _canvasGroup;
    
    public override void GetPanelComponents()
    {
        if (_canvasGroup == null) 
            _canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void SetDataToPanel(object data)
    {
        throw new System.NotImplementedException();
    }

    public override void OpenPanel()
    {
        if (_canvasGroup == null)
            GetPanelComponents();
        
        throw new System.NotImplementedException();
    }

    public override void ClosePanel()
    {
        if (_canvasGroup == null)
            GetPanelComponents();
        
        throw new System.NotImplementedException();
    }
}
