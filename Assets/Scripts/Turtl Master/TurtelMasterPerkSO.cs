using UnityEngine;

[CreateAssetMenu(fileName = "TurtelMasterPerkSO", menuName = "Turtel Master/TurtelMasterPerkSO")]
public class TurtelMasterPerkSO : ScriptableObject
{
    public string TurtleMasterPerkName;
    public TurtleMasterPerk TurtleMasterPerk;
    [TextArea(25, 50)]
    public string TurtleMasterPerkDescription;
    public int percentageValue;

}
