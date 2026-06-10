using UnityEngine;
using TMPro;

public class PanelNotifiication : Panel
{
    [SerializeField] private TMP_Text treasureDetail;

    [SerializeField] private CanvasGroup _canvasGroup;


    public override void ClosePanel()
    {
        if (this == null)
            return;

        if (_canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup NULL -> Regetting");
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        _canvasGroup.alpha = 0;
    }

    public override void OpenPanel()
    {
        if (this == null)
            return;

        if (_canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup NULL -> Regetting");
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        _canvasGroup.alpha = 1;
    }

    public override void GetPanelComponents()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void SetDataToPanel(object data)
    {
        var currencyData = (CurrecyData)data;

        string currencyName = currencyData.CurrencyType == TreasureRandomItemType.Seacoene ? "SeaCoin" : "AdaptPoint";

        treasureDetail.text = $"You got {currencyData.Amount} \n {currencyName}!";
    }
}
