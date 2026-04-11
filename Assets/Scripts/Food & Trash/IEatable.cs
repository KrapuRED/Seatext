using UnityEngine;

public interface IEatable
{
    public FoodSize foodSize { get; set; }

    public void Eat(FishType fishType);
}
