using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FishOS", menuName = "FishData/FishOS")]
public class FishOS : ScriptableObject
{
    public string fishName;
    public string fishID;
    public Sprite fishSprite;

    [Header("State & Condition Fish")]
    public List<Transition> StateConditionFish = new List<Transition>();
}
