using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Player Data/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    public FishSO baseFishStats;   // reference, so player still gets fish-type stats
    public int startingTrash;
    public float startingHunger;
    public float maxHunger;
}
