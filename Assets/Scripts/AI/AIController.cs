using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AttackType
{
    GroundPound,
}

public enum AIState
{
    Idle,
    Attacking
}

public class AIController : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 3.0f;
    [SerializeField] private GroundPoundAi _groundPoundAi;

    private AIState _currentState = AIState.Idle;
    private float _nextAttackTime;

    void Start()
    {
        _groundPoundAi = GetComponent<GroundPoundAi>();

        //if (animator == null)
        //{
        //    animator = GetComponent<Animator>();
        //}

        // Start the AI behavior coroutine
        StartCoroutine(AIBehaviorRoutine());
    }

    IEnumerator AIBehaviorRoutine()
    {
        while (true)
        {
            switch(_currentState)
            {
                case AIState.Idle:
                    yield return StartCoroutine(IdleRoutine());
                    break;

                case AIState.Attacking:
                    yield return StartCoroutine(AttackingRoutine());
                    break;
            }
        }
    }

    IEnumerator IdleRoutine()
    {
        // Wait for the next attack time
        yield return new WaitForSeconds(_attackCooldown);

        // Decide on an attack
        DecideAttack();
    }

    void DecideAttack()
    {
        // Decide which attack to use
        AttackType chosenAttack = (AttackType)Random.Range(0, System.Enum.GetValues(typeof(AttackType)).Length);

        _currentState = AIState.Attacking;

        // Execute the chosen attack
        switch (chosenAttack)
        {
            case AttackType.GroundPound:
                StartCoroutine(GroundPoundSequence());
                break;
                // Add other cases for different attacks
        }
    }

    IEnumerator GroundPoundSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        // Wait for the animation to start
        yield return new WaitForSeconds(0.5f); // Adjust timing to match animation

        _groundPoundAi.PerformGroundPound();

        // Return to Idle state after attack
        _currentState = AIState.Idle;

        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator AttackingRoutine()
    {
        // Here you can add any additional logic for the attacking state
        // For now, we just wait until the current attack finishes

        // Wait until the current attack is finished
        while (_currentState == AIState.Attacking)
        {
            yield return null;
        }
    }
}
