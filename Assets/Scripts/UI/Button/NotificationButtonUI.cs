using UnityEngine;

public class NotificationButtonUI : MonoBehaviour
{
    public static void OnConfirmButtonClick()
    {
        Debug.Log("Confirm button clicked!");
        PanelManager.instance.ClosePanelByPanelType(PanelType.PanelNotifiication);
    }
}
