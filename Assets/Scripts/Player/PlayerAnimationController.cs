using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private CharacterMovement _characterMovement;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat("Right", _characterMovement.CurrentDirection);
    }


}
