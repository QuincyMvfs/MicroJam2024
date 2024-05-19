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
    SpiralShooting,
    SpinningLaser,
    BulletShower,
    CircleTravellingProjectile,
    SpawnObsticles,
}

public enum AIState
{
    Idle,
    Attacking
}

[RequireComponent(typeof(SpiralShooter))]
[RequireComponent(typeof(BulletShower))]
[RequireComponent(typeof(SpawnObsticles))]

public class AIController : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 3.0f;

    private ZoneExplosion _zoneExplosion;
    private Groundpound _groundpound;
    private OneShotCoverAttack _oneShotCoverAttack;
    private SpiralShooter _spiralShooter;
    private BulletShower _bulletShower;
    private SpinningLasers _spinningLasers;
    private CircleTravellingProjectile _circleTravellingProjectile;
    private SpawnObsticles _spawnObsticles;

    private AIState _currentState = AIState.Idle;

    void Start()
    {
        _zoneExplosion = GetComponent<ZoneExplosion>();
        _groundpound = GetComponent<Groundpound>();
        _oneShotCoverAttack = GetComponent<OneShotCoverAttack>();
        _spiralShooter = GetComponent<SpiralShooter>();
        _spinningLasers = GetComponent<SpinningLasers>();
        _bulletShower = GetComponent<BulletShower>();
        _circleTravellingProjectile = GetComponent<CircleTravellingProjectile>();
        _spawnObsticles = GetComponent<SpawnObsticles>();

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
        //AttackType chosenAttack = AttackType.SpawnObsticles;
        _currentState = AIState.Attacking;

        //Execute the chosen attack
        switch (chosenAttack)
        {
            case AttackType.GroundPound:
                StartCoroutine(GroundPoundSequence());
                break;
            case AttackType.ZoneExplosion:
                StartCoroutine(ZoneExplosionSequence());
                break;
            case AttackType.OneShotCoverAttack:
                StartCoroutine(OneShotSequence());
                break;
            case AttackType.SpiralShooting:
                StartCoroutine(SpiralShootingSequence());
                break;
            case AttackType.SpinningLaser:
                StartCoroutine(LaserSpinningSequence());
                break;
            case AttackType.BulletShower:
                StartCoroutine(BulletShowerSequence());
                break;
            case AttackType.CircleTravellingProjectile:
                StartCoroutine(CircleTravellingProjectileSequence());
                break;
            case AttackType.SpawnObsticles:
                StartCoroutine(SpawnObsticleSequence());
                break;
                //Add other cases for different attacks
        }
    }
    IEnumerator GroundPoundSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f); 
        _groundpound.StartExplosion();

        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator ZoneExplosionSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f);
        _zoneExplosion.PerformGroundPound();

        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator OneShotSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f);
        _oneShotCoverAttack.ExecuteOneShot();

        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator SpiralShootingSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f);
        _spiralShooter.StartSpinning();

        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator LaserSpinningSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f);
        _spinningLasers.StartSpinningLasers();

        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator BulletShowerSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f);
        _bulletShower.StartSpinning();

        // Restart the AI behavior routine
        yield return null;
    }
    IEnumerator CircleTravellingProjectileSequence()
    {
        yield return new WaitForSeconds(0.5f);
        _circleTravellingProjectile.LaunchCircleTravellingProjectile();
        
        yield return null;
    }

    IEnumerator SpawnObsticleSequence()
    {
        yield return new WaitForSeconds(0.5f);
        _spawnObsticles.StartSpawningObsticles();

        yield return null;
    }

    // Called from the attack classes
    public void ResetToIdle()
    {
        _currentState = AIState.Idle;
    }
}
