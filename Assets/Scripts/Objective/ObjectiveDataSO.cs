using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveDataSO", menuName = "Objective Data/ObjectiveDataSO")]
public class ObjectiveDataSO : ScriptableObject
{
    public string objectiveName;
    public float timerObjetive;
    public int countObjectives;
    public FishType fishType;
}
