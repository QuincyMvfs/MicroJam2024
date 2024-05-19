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
    [SerializeField] private float _rotationSpeed = 2.0f;
    [SerializeField] private GameObject _bossMesh;

    private ZoneExplosion _zoneExplosion;
    private GroundPoundNew _groundpound;
    private OneShotCoverAttack _oneShotCoverAttack;
    private SpiralShooter _spiralShooter;
    private BulletShower _bulletShower;
    private SpinningLasers _spinningLasers;
    private CircleTravellingProjectile _circleTravellingProjectile;
    private SpawnObsticles _spawnObsticles;
    private PlayerController _playerController;

    private AIState _currentState = AIState.Idle;

    private int _index;

    void Start()
    {
        _zoneExplosion = GetComponent<ZoneExplosion>();
        _groundpound = GetComponent<GroundPoundNew>();
        _oneShotCoverAttack = GetComponent<OneShotCoverAttack>();
        _spiralShooter = GetComponent<SpiralShooter>();
        _spinningLasers = GetComponent<SpinningLasers>();
        _bulletShower = GetComponent<BulletShower>();
        _circleTravellingProjectile = GetComponent<CircleTravellingProjectile>();
        _spawnObsticles = GetComponent<SpawnObsticles>();

        _playerController = FindObjectOfType<PlayerController>();

        // Start the AI behavior coroutine
        StartCoroutine(AIBehaviorRoutine());
    }

    private void Update()
    {
        if (_playerController != null && _bossMesh != null)
        {
            LookAtPlayer();
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (_playerController.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        lookRotation *= Quaternion.Euler(0, 180, 0);
        _bossMesh.transform.rotation = Quaternion.Slerp(_bossMesh.transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
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
        BossPattern();
    }

    IEnumerator AttackingRoutine()
    {
        // Wait until the current attack is finished
        while (_currentState == AIState.Attacking)
        {
            yield return null;
        }
    }

    private void BossPattern()
    {
        // Decide which attack to use
        //   AttackType chosenAttack = (AttackType)Random.Range(0, System.Enum.GetValues(typeof(AttackType)).Length);
        
        //AttackType chosenAttack = AttackType.SpawnObsticles;
        _currentState = AIState.Attacking;

        //Execute the chosen attack
        switch (_index)
        {
            case 0:
                StartCoroutine(GroundPoundSequence());
                StartCoroutine(WaitingTime());
                break;
            case 1:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 2:
                StartCoroutine(LaserSpinningSequence());
                StartCoroutine(WaitingTime());
                break;
            case 3:
                StartCoroutine(BulletShowerSequence());
                StartCoroutine(WaitingTime());
                break;
            case 4:
                StartCoroutine(GroundPoundSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 5:
                StartCoroutine(SpiralShootingSequence());
                StartCoroutine(WaitingTime());
                break;
            case 6:
                StartCoroutine(BulletShowerSequence());
                StartCoroutine(WaitingTime());
                break;
            case 7:
                StartCoroutine(LaserSpinningSequence());
                StartCoroutine(WaitingTime());
                break;
            case 8:
                StartCoroutine(GroundPoundSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 9:
                StartCoroutine(BulletShowerSequence());
                StartCoroutine(WaitingTime());
                break;
            case 10:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 11:
                StartCoroutine(SpiralShootingSequence());
                StartCoroutine(WaitingTime());
                break;
            case 12:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 13:
                StartCoroutine(LaserSpinningSequence());
                StartCoroutine(WaitingTime());
                break;
            case 14:
                StartCoroutine(BulletShowerSequence());
                StartCoroutine(WaitingTime());
                break;
            case 15:
                StartCoroutine(GroundPoundSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 16:
                StartCoroutine(SpiralShootingSequence());
                StartCoroutine(WaitingTime());
                break;
            case 17:
                StartCoroutine(CircleTravellingProjectileSequence());
                StartCoroutine(WaitingTime());
                break;
            case 18:
                StartCoroutine(LaserSpinningSequence());
                StartCoroutine(WaitingTime());
                break;
            case 19:
                StartCoroutine(GroundPoundSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 20:
                StartCoroutine(BulletShowerSequence());
                StartCoroutine(WaitingTime());
                break;
            case 21:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 22:
                StartCoroutine(SpiralShootingReverseSequence());
                StartCoroutine(WaitingTime());      //Health Bar Stop Decreasing
                break;
            case 23:                                //Transition
                StartCoroutine(OneShotSequence());
                StartCoroutine(WaitingTime());
                break;
            case 24:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 25:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 26:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 27:
                StartCoroutine(SpiralShootingSequence()); //Health Bar Decreasing again
                StartCoroutine(WaitingTime());
                break;
            case 28:
                StartCoroutine(SpawnObsticleSequence());
                StartCoroutine(BulletShowerSequence());
                StartCoroutine(WaitingTime());
                break;
            case 29:
                StartCoroutine(LaserSpinningSequence());
                StartCoroutine(GroundPoundSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 30:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 31:
                StartCoroutine(SpiralShootingSequence());
                StartCoroutine(WaitingTime());
                break;
            case 32:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingTime());
                break;
            case 33:
                StartCoroutine(BulletShowerSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 34:
                StartCoroutine(WaitingShortTime());
                break;
            case 35:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingShortTime());
                break;
            case 36:                
                StartCoroutine(SpawnObsticleSequence());
                StartCoroutine(BulletShowerSequence());
                break;
                //Add other cases for different attacks

        }
        _index++;
    }

    IEnumerator WaitingTime()
    {
        yield return new WaitForSeconds(10.0f);
        BossPattern();
        // Restart the AI behavior routine
    }

    IEnumerator WaitingShortTime()
    {
        yield return new WaitForSeconds(3.0f);
        BossPattern();
        // Restart the AI behavior routine
    }

    IEnumerator GroundPoundSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f); 
        _groundpound.PerformGroundPound();

        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator ZoneExplosionSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f);
        _zoneExplosion.PerformZoneExplosion();

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

    IEnumerator SpiralShootingReverseSequence()
    {
        //TODO::_animator.SetTrigger("GroundPound");

        yield return new WaitForSeconds(0.5f);
        _spiralShooter.StartSpinningReverse();

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
