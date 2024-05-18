using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum AttackType
{
    GroundPound,
    ZoneExplosion,
    OneShotCoverAttack,
}

public enum AIState
{
    Idle,
    Attacking
}

public class AIController : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 3.0f;
    [SerializeField] private ZoneExplosion _zoneExplosion;
    [SerializeField] private Groundpound _groundpound;
    [SerializeField] private OneShotCoverAttack _oneShotCoverAttack;

    private AIState _currentState = AIState.Idle;
    private float _nextAttackTime;

    void Start()
    {
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

    IEnumerator AttackingRoutine()
    {
        // Wait until the current attack is finished
        while (_currentState == AIState.Attacking)
        {
            yield return null;
        }
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
                StartCoroutine(OneShotSequence());
                break;
            case AttackType.ZoneExplosion:
                StartCoroutine(OneShotSequence());
                break;
            case AttackType.OneShotCoverAttack:
                StartCoroutine(OneShotSequence());
                break;

                // Add other cases for different attacks
        }
    }

    IEnumerator ZoneExplosionSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f);
        _zoneExplosion.PerformGroundPound();

        // Return to Idle state after attack
        _currentState = AIState.Idle;

        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator GroundPoundSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f); 
        _groundpound.StartExplosion();

        // Return to Idle state after attack
        _currentState = AIState.Idle;

        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator OneShotSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f);
        _oneShotCoverAttack.ExecuteOneShot();

        // Return to Idle state after attack
        _currentState = AIState.Idle;

        // Restart the AI behavior routine
        yield return null;
    }


}
