using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private float _xMovementSpeed = 5.0f;
    [SerializeField] private float _yMovementSpeed = 10.0f;

    protected IEnumerator _currentState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        while (true)
        {
            this.transform.position += new Vector3(0, 0, (_yMovementSpeed * Time.fixedDeltaTime));
            yield return null;
        }
    }

    protected IEnumerator MoveBackward()
    {
        while (true)
        {
            this.transform.position -= new Vector3(0, 0, (_yMovementSpeed * Time.fixedDeltaTime));
            yield return null;
        }
    }

    protected IEnumerator MoveLeft()
    {
        while (true)
        {
            this.transform.position -= new Vector3((_xMovementSpeed * Time.fixedDeltaTime), 0, 0);
            yield return null;
        }
    }

    protected IEnumerator MoveRight()
    {
        while (true)
        {
            this.transform.position += new Vector3((_xMovementSpeed * Time.fixedDeltaTime), 0, 0);
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
