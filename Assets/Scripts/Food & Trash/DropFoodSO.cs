using UnityEngine;

public enum FoodType
{
    none,
    Trash,
    Pellet,
    Goldenpellet
}

[CreateAssetMenu(fileName = "DropFoodSO", menuName = "Foods/DropFoodSO")]
public class DropFoodSO : ScriptableObject
{
    public string foodName;
    public string foodID;
    public FoodType foodType;
    public float gainStatus;
}
