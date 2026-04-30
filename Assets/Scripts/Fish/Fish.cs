using UnityEngine;

public enum FishType
{
    None,
    Player,
    Tiny,
    Small,
    Big
}

[System.Serializable]
public abstract class FishContex
{
    public GameObject fishObject;
    public Transform fishPosition;
    public FishMovement fishMovement;
    public FishEyeSight fishEyeSight;
    public FishMouth fishMouth;
    public FishSpeed fishSpeed;
    public Fish fish;
}

public class Fish : MonoBehaviour
{
    [Header("General Fish Config")]
    [SerializeField] private FishSO _fishData;
    [SerializeField] private int _fishIndex;
    [SerializeField] private FishMovement _fishMovement;
    [SerializeField] private FishEyeSight _fishEyeSight;
    [SerializeField] private FishSpeed _fishSpeed;
    [SerializeField] private FishMouth _fishMouth;
    [SerializeField] private bool _isBeenHunted;
    [SerializeField] private FishType _fishType;

    [Header("Config Eating")]
    [SerializeField] protected Transform mouthPosition;
    [SerializeField] protected float eatRange;

    public FishSO FishData => _fishData;
    public FishMovement FishMovement => _fishMovement;
    public FishEyeSight FishEyeSight => _fishEyeSight;
    public FishSpeed    FishSpeed => _fishSpeed;
    public FishMouth FishMouth => _fishMouth;
    public bool IsBeenHunted => _isBeenHunted;
    public FishType FishType => _fishType;
    public int FishIndex => _fishIndex;
    public FishContex Contex { get;  set; }

    public virtual void OnEating()
    {

    }

    public void SetFishData(FishSO data, int fishIndex)
    {
        _fishData = data;
        _fishIndex = fishIndex;
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
