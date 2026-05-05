using System;
using UnityEngine;

public class FishAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnHandlingMovementAnimation(float distance)
    {
        Debug.Log($"[FishAnimation - OnHandlingMovementAnimation] is playing swim!");
        _animator.SetFloat("MoveToPos", distance);
    }

    public void OnHandlingTurningAnimation()
    {
        
    }
}
