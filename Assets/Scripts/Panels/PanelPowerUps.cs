using System;
using UnityEngine;

public class PanelPowerUps : Panel
{
    
    private CanvasGroup  _canvasGroup;

    private void Start()
    {
        if (_canvasGroup.alpha == 1)
            PanelManager.instance.OpenPanelByTypePanel(this.panelType);
    }

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
    
    public void ClosingPanel()
    {
        PanelManager.instance.ClosePanelByPanelType(this.panelType);
    }
}
