using UnityEngine;
using System.Collections.Generic;

public enum FishBehavior
{
    None,
    Normal,
    Aggressive,
    Passive
}

public enum FoodSize
{
    None, 
    Small,
    Large
}

[CreateAssetMenu(fileName = "FishOS", menuName = "FishData/FishOS")]
public class FishOS : ScriptableObject
{
    public string fishName;
    public string fishID;
    public Sprite fishSprite;
    public FoodSize fishSize;
    public FishBehavior fishBehavior;

    [Header("Fish Movement")]
    public float speedFish;

    [Header("State & Condition Fish")]
    public List<DataStateCondtion> dataStateCondtions = new List<DataStateCondtion>();
}
