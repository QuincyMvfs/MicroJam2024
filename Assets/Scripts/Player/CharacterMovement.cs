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
    [SerializeField] private GameObject _lookTarget;
    [SerializeField] private Transform _playerMesh;

    [Header("Speeds")]
    [SerializeField] private float _xMovementSpeed = 5.0f;
    [SerializeField] private float _stepSpeed = 5.0f;

    [Header("Step Details")]
    [SerializeField] private float _stepDelay = 0.05f; 
    [SerializeField] private int _maxSteps = 4;
    [SerializeField] private LayerMask _occlusionMask;
    [SerializeField] private float _sphereCastRadius = 0.5f;

    public int CurrentStep => _currentStep;

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
        Vector3 LookPosition = _lookTarget.transform.position - transform.position;
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
                Ray obstacleRay = new Ray();
                obstacleRay.origin = _playerMesh.transform.position;
                obstacleRay.direction = _playerMesh.transform.forward;
                if (!Physics.SphereCast(obstacleRay.origin, _sphereCastRadius, obstacleRay.direction, 
                    out RaycastHit hitInfo, 2.0f, _occlusionMask))
                {
                    _currentStep++;
                    _nextStepTime = Time.time + _stepDelay;
                    this.transform.Translate(new Vector3(0, 0, _stepSpeed), Space.Self);
                }
                else
                {
                    Debug.Log(hitInfo.collider.gameObject.name);
                    Debug.DrawRay(obstacleRay.origin, obstacleRay.direction, Color.red, 5.0f);
                }
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
                Ray obstacleRay = new Ray();
                obstacleRay.origin = _playerMesh.transform.position;
                obstacleRay.direction = -_playerMesh.transform.forward;
                if (!Physics.SphereCast(obstacleRay.origin, _sphereCastRadius, obstacleRay.direction, 
                    out RaycastHit hitInfo, 2.0f, _occlusionMask))
                {
                    _currentStep--;
                    _nextStepTime = Time.time + _stepDelay;
                    this.transform.Translate(new Vector3(0, 0, -_stepSpeed), Space.Self);
                }
                else
                {
                    Debug.Log(hitInfo.collider.gameObject.name);
                    Debug.DrawRay(obstacleRay.origin, obstacleRay.direction, Color.red, 5.0f);
                }
            }
        }
    }

    protected IEnumerator MoveLeft()
    {
        while (true)
        {
            Ray obstacleRay = new Ray();
            obstacleRay.origin = _playerMesh.transform.position;
            obstacleRay.direction = -_playerMesh.transform.right;
            if (!Physics.Raycast(obstacleRay, out RaycastHit hitInfo, 1f, _occlusionMask))
            {
                this.transform.Translate(new Vector3(-(_xMovementSpeed * Time.fixedDeltaTime), 0, 0), Space.Self);
            }

            yield return null;
        }
    }

    protected IEnumerator MoveRight()
    {
        while (true)
        {
            Ray obstacleRay = new Ray();
            obstacleRay.origin = _playerMesh.transform.position;
            obstacleRay.direction = _playerMesh.transform.right;
            if (!Physics.Raycast(obstacleRay, out RaycastHit hitInfo, 1f, _occlusionMask))
            {
                this.transform.Translate(new Vector3((_xMovementSpeed * Time.fixedDeltaTime), 0, 0), Space.Self);
            }

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
