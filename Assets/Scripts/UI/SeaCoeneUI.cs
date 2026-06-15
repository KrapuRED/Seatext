using UnityEngine;
using TMPro;

public class SeaCoeneUI : CurrencyUI
{
    [SerializeField] private TMP_Text seaCoeneValueText;

    private void OnEnable()
    {
        GameEvents.OnUpdateCurrecyUI.AddListener(UpdateCurruccnyValue);
    }

    private void OnDisable()
    {
        OnRemoveListener();
    }

    private void OnDestroy()
    {
        OnRemoveListener();
    }

    void OnRemoveListener()
    {
        GameEvents.OnUpdateCurrecyUI.RemoveListener(UpdateCurruccnyValue);

    }

    public override void UpdateCurruccnyValue(CurrecyData currecyData)
    {
        if (currecyData.currencyType != CurrencyType.Seacoene)
            return;

        seaCoeneValueText.text = currecyData.Amount.ToString();
    }
}