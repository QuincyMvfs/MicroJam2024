using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterMovement))]

public class PlayerController : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    [SerializeField] private PauseFunction _pauseFunction;
    [SerializeField] private Button _unPauseButton;

    private bool _isMovingLeft = false;
    private bool _isMovingRight = false;
    private MovementDirections _currentDirection = MovementDirections.Stop;

    public bool IsPaused => _pauseFunction.IsPaused;

    // Start is called before the first frame update
    void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    public void StopMovement()
    {
        _characterMovement.ChangeMovementState(MovementDirections.Stop);
        _isMovingLeft = false;
        _isMovingRight = false;
    }

    private void OnMoveForward()
    {
        if (IsPaused) return;

        _characterMovement.ChangeMovementState(MovementDirections.Forward);
    }

    private void OnMoveBackward()
    {
        if (IsPaused) return;

        _characterMovement.ChangeMovementState(MovementDirections.Backward);
    }

    private void OnMoveLeft()
    {
        if (IsPaused) return;

        _isMovingLeft = !_isMovingLeft;

        if (_isMovingLeft)
        {
            _currentDirection = MovementDirections.Left;
            _characterMovement.ChangeMovementState(MovementDirections.Left);
        }
        else if (_currentDirection == MovementDirections.Left)
        {
            _characterMovement.ChangeMovementState(MovementDirections.Stop);
        }
    }

    private void OnMoveRight()
    {
        if (IsPaused) return;

        _isMovingRight = !_isMovingRight;
        if (_isMovingRight)
        {
            _currentDirection = MovementDirections.Right;
            _characterMovement.ChangeMovementState(MovementDirections.Right);
        }
        else if (_currentDirection == MovementDirections.Right)
        {
            _characterMovement.ChangeMovementState(MovementDirections.Stop);
        }
    }

    private void OnPause()
    {
        _pauseFunction.Pause();
    }
}
