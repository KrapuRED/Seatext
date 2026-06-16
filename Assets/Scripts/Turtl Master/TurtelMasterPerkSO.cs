using UnityEngine;

[CreateAssetMenu(fileName = "TurtelMasterPerkSO", menuName = "Turtel Master/TurtelMasterPerkSO")]
public class TurtelMasterPerkSO : ScriptableObject
{
    public string TurtelMasterPerkName;
    public TurtleMasterPerk TurtelMasterPerk;
    [TextArea(25, 50)]
    public string TurtelMasterPerkDescriptionr;
    public int percentageValue;
    public int costValue;
}
