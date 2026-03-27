using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private FishOS _fishData;
    [SerializeField] private FishMovement _fishMovement;

    public FishOS fishData => _fishData;
    public FishMovement fishMovement => _fishMovement;

}
