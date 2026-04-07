using UnityEngine;

public interface IEatable
{
    public bool IsEdible { get; set; }

    public void Eat();
}
