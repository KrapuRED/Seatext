using UnityEngine;

public class ButtonCloseLevelNodeInfoTypeBox : ButtonTypeBox
{
   public override void OnInkoveEvent()
   {
      GameEvents.OnClosePanelByID.Invoke(PanelID);
      GameEvents.OnSelectedPreviousLevelNode.Invoke();
   }
}
