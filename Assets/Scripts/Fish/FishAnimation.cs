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
        _animator.SetFloat("MoveToPos", distance);
    }
}
