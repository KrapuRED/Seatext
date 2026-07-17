using System;
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

    }

    public override void OpenPanel()
    {
        if (_canvasGroup == null)
            GetPanelComponents();
        
        _canvasGroup.alpha = 1;
    }

    public override void ClosePanel()
    {
        if (_canvasGroup == null)
            GetPanelComponents();
        
        _canvasGroup.alpha = 0;
    }

    public void OpenPowerUpPanel()
    {
        PanelManager.instance.OpenPanelByTypePanel(this.panelType);
    }
    
    public void ClosingPowerUpPanel()
    {
        PanelManager.instance.ClosePanelByPanelType(this.panelType);
    }
}
