using UnityEngine;
using System.Collections.Generic;

public enum FishBehavior
{
    none,
    normal,
    aggressive,
    passive
}

public enum FishType
{
    none, 
    small,
    Big
}

[CreateAssetMenu(fileName = "FishOS", menuName = "FishData/FishOS")]
public class FishOS : ScriptableObject
{
    public string fishName;
    public string fishID;
    public Sprite fishSprite;
    public FishType fishType;
    public FishBehavior fishBehavior;

    [Header("Fish Movement")]
    public float speedFish;

    [Header("State & Condition Fish")]
    public List<DataStateCondtion> dataStateCondtions = new List<DataStateCondtion>();
}
