using UnityEngine;

public class PanelRetry : Panel
{
    private CanvasGroup  _canvasGroup;
    
    public override void GetPanelComponents()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();

        if (_canvasGroup.alpha == 1)
        {
            Debug.Log($"[{gameObject.name}] Canvas Group is active");
            PanelManager.instance.OpenPanelByTypePanel(panelType);
        }
    }

    public override void SetDataToPanel(object data)
    {
        throw new System.NotImplementedException();
    }

    public override void OpenPanel()
    {
        _canvasGroup.alpha = 1;
    }

    public override void ClosePanel()
    {
        _canvasGroup.alpha = 0;
    }
    
    public void ConfirmRetry()
    {
        Debug.Log($"[{gameObject.name}] Confirm Retry");
        GameManager.instance.RestartGame();
        
        PanelManager.instance.ClosePanelByPanelType(panelType);
    }

    public void ConfirmExitGame()
    {
        Debug.Log($"[{gameObject.name}] Confirm Exit Game!");
    }
}
