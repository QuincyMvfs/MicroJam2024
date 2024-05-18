using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMovement))]

public class PlayerController : MonoBehaviour
{
    private CharacterMovement _characterMovement;

    private bool _isMovingForward = false;
    private bool _isMovingBackward = false;
    private bool _isMovingLeft = false;
    private bool _isMovingRight = false;
    private MovementDirections _currentDirection = MovementDirections.Stop;

    // Start is called before the first frame update
    void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void OnMoveForward()
    {
        _characterMovement.ChangeMovementState(MovementDirections.Forward);
    }

    private void OnMoveBackward()
    {
        _characterMovement.ChangeMovementState(MovementDirections.Backward);
    }

    private void OnMoveLeft()
    {
        _isMovingLeft = !_isMovingLeft;

        if (_isMovingLeft)
        {
            _currentDirection = MovementDirections.Left;
            Debug.Log("Move Left");
            _characterMovement.ChangeMovementState(MovementDirections.Left);
        }
        else if (_currentDirection == MovementDirections.Left)
        {
            Debug.Log("Stop");
            _characterMovement.ChangeMovementState(MovementDirections.Stop);
        }
    }

    private void OnMoveRight()
    {
        _isMovingRight = !_isMovingRight;
        if (_isMovingRight)
        {
            _currentDirection = MovementDirections.Right;
            Debug.Log("Move Right");
            _characterMovement.ChangeMovementState(MovementDirections.Right);
        }
        else if (_currentDirection == MovementDirections.Right)
        {
            Debug.Log("Stop");
            _characterMovement.ChangeMovementState(MovementDirections.Stop);
        }
    }
}
