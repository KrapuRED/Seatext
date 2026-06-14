using UnityEngine;

public class ButtonDoneLevelNodeTypeBox : ButtonTypeBox
{
    public override void OnInkoveEvent()
    {
        GameEvents.OnButtonTypeBoxComplete.Invoke(ButtonTypeBoxContext);
        
        //save Status Data
        GameEvents.OnSaveCurrentStatus.Invoke();
    }
}
