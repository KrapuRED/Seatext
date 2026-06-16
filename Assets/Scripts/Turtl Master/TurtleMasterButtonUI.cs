using UnityEngine;

public class TurtleMasterButtonUI : MonoBehaviour
{
    [Header("Turtel Master Config")]
    [SerializeField] private TurtelMasterPerkSO perkData;

    public TurtelMasterPerkSO PerkData => perkData;

    public void OnButtonClicked()
    {
        if (perkData == null)
        {
            Debug.Log($"Perk Data Missing, Please Check the Config of the Button");
            return;
        }

        TurtleMasterManager.Instance.ApplyeTurtleMaster(perkData);
    }
}
