using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]

public class CharacterMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject LookTarget;

    [Header("Speeds")]
    [SerializeField] private float _xMovementSpeed = 5.0f;
    [SerializeField] private float _stepSpeed = 100.0f;

    [Header("Step Details")]
    [SerializeField] private float _stepDelay = 0.05f; 
    [SerializeField] private int _maxSteps = 4;

    private Rigidbody _rb;

    private float _nextStepTime = 0;
    private int _currentStep = 0;
    protected IEnumerator _currentState;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 LookPosition = LookTarget.transform.position - transform.position;
        LookPosition.y = 0;
        Quaternion LookRotation = Quaternion.LookRotation(LookPosition);
        transform.rotation = LookRotation;

    }

    public void ChangeState(IEnumerator newState)
    {
        if (_currentState != null) StopCoroutine(_currentState);

        _currentState = newState;
        StartCoroutine(_currentState);
    }

    public void ChangeMovementState(MovementDirections Direction)
    {
        switch (Direction)
        {
            case MovementDirections.Forward:
                MoveForward();
                break;
            case MovementDirections.Backward:
                MoveBackward();
                break;
            case MovementDirections.Left:
                ChangeState(MoveLeft());
                break;
            case MovementDirections.Right:
                ChangeState(MoveRight());
                break;
            case MovementDirections.Stop:
                ChangeState(Stop());
                break;
        }
    }

    protected void MoveForward()
    {
        if (_currentStep >= _maxSteps)
        {
            _currentStep = _maxSteps;
        }
        else
        {
            if (Time.time > _nextStepTime)
            {
                _currentStep++;
                _nextStepTime = Time.time + _stepDelay;
                this.transform.Translate(new Vector3(0, 0, (_stepSpeed * Time.fixedDeltaTime)), Space.Self);
            }
        }
    }

    protected void MoveBackward()
    {
        if (_currentStep <= 0)
        {
            _currentStep = 0;
        }
        else
        {
            if (Time.time > _nextStepTime)
            {
                _currentStep--;
                _nextStepTime = Time.time + _stepDelay;
                this.transform.Translate(new Vector3(0, 0, -(_stepSpeed * Time.fixedDeltaTime)), Space.Self);
            }
        }
    }

    protected IEnumerator MoveLeft()
    {
        while (true)
        {
            this.transform.Translate(new Vector3(-(_xMovementSpeed * Time.fixedDeltaTime), 0, 0), Space.Self);
            yield return null;
        }
    }

    protected IEnumerator MoveRight()
    {
        while (true)
        {
            this.transform.Translate(new Vector3((_xMovementSpeed * Time.fixedDeltaTime), 0, 0), Space.Self);
            yield return null;
        }
    }

    protected IEnumerator Stop()
    {
        yield return null;
    }


}

public enum MovementDirections
{
    Forward, Backward, Left, Right, Stop
}
