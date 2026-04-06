using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishOS _fishData;
    [SerializeField] private FishMovement _fishMovement;
    [SerializeField] private FishEyeSight _fishEyeSight;
    [SerializeField] private bool _isBeenHunted;

    public FishOS fishData => _fishData;
    public FishMovement fishMovement => _fishMovement;
    public FishEyeSight fishEyeSight => _fishEyeSight;
    public bool isBeenHunted => _isBeenHunted;

    public virtual void SetBeenHunted(bool isBeenHunted, Fish curerntFish = null)
    {
        _isBeenHunted = isBeenHunted;
    }
}
