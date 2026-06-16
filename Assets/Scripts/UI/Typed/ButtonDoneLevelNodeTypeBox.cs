using UnityEngine;

public class ButtonDoneLevelNodeTypeBox : ButtonTypeBox
{
    public override void OnInkoveEvent()
    {
        GameEvents.OnButtonTypeBoxComplete.Invoke(ButtonTypeBoxContext);
        
        //save Status Data
        GameEvents.OnSaveCurrentStatus.Invoke();

        CurrecyData currency = new CurrecyData(CurrencyType.Seacoene, 1000);

        CurrencyManager.instance.UpdateCurrency(currency);
    }
}
