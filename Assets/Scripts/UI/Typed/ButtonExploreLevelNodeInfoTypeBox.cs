using UnityEngine;

public class ButtonExploreLevelNodeInfoTypeBox : ButtonTypeBox
{
    public override void OnInkoveEvent()
    {
        GameEvents.OnButtonTypeBoxComplete.Invoke(ButtonTypeBoxContext);
    }
}
