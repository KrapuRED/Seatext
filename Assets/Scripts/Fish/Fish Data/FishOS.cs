using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FishOS", menuName = "FishData/FishOS")]
public class FishOS : ScriptableObject
{
    public string fishName;
    public string fishID;
    public Sprite fishSprite;

    [Header("Fish Movement")]
    public float speedFish;

    [Header("State & Condition Fish")]
    public List<DataStateCondtion> dataStateCondtions = new List<DataStateCondtion>();
}
