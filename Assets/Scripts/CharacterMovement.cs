using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject LookTarget;

    [SerializeField] private float _xMovementSpeed = 5.0f;
    [SerializeField] private float _stepSpeed = 100.0f; 
    [SerializeField] private float _stepDelay = 0.2f; 
    [SerializeField] private int _maxSteps = 4;

    private int _currentStep = 0;
    protected IEnumerator _currentState;

    // Start is called before the first frame update
    void Start()
    {
        
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
                ChangeState(MoveForward());
                break;
            case MovementDirections.Backward:
                ChangeState(MoveBackward());
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

    protected IEnumerator MoveForward()
    {
        _currentStep++;
        if (_currentStep >= _maxSteps)
        {
            _currentStep = _maxSteps;
            yield break;
        }
        else
        {
            this.transform.Translate(new Vector3(0, 0, (_stepSpeed * Time.fixedDeltaTime)), Space.Self);
            yield return new WaitForSeconds(_stepDelay);
        }
    }

    protected IEnumerator MoveBackward()
    {
        _currentStep--;
        if (_currentStep <= 0)
        {
            _currentStep = 0;
            yield break;
        }
        else
        {
            this.transform.Translate(new Vector3(0, 0, -(_stepSpeed * Time.fixedDeltaTime)), Space.Self);
            yield return new WaitForSeconds(_stepDelay);
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
