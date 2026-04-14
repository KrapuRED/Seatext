using UnityEngine;

public enum FishType
{
    None,
    Player,
    Enemy
}

public class Fish : MonoBehaviour
{
    [SerializeField] private FishOS _fishData;
    [SerializeField] private FishMovement _fishMovement;
    [SerializeField] private FishEyeSight _fishEyeSight;
    [SerializeField] private FishSpeed _fishSpeed;
    [SerializeField] private bool _isBeenHunted;
    [SerializeField] private FishType _fishType;

    public FishOS fishData => _fishData;
    public FishMovement fishMovement => _fishMovement;
    public FishEyeSight fishEyeSight => _fishEyeSight;
    public FishSpeed    fishSpeed => _fishSpeed;
    public bool isBeenHunted => _isBeenHunted;
    public FishType fishType => _fishType;

    public void SetFishData(FishOS data)
    {
        _fishData = data;
    }

    public virtual void SetBeenHunted(bool isBeenHunted, Fish curerntFish = null)
    {
        _isBeenHunted = isBeenHunted;
    }

    public virtual void DodgeAttackFish(Vector2 attackDirection)
    {
        Debug.Log($"[{gameObject.name} - DodgeEnemy] Try to Dodge Enemy Attack!");
    }
}
