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
    [SerializeField] private float _rotationSpeed = 1.0f;
    [SerializeField] private GameObject _bossMesh;

    private ZoneExplosion _zoneExplosion;
    private Groundpound _groundpound;
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
        _groundpound = GetComponent<Groundpound>();
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
        Debug.Log("SDfsd");
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
        //   // Decide which attack to use
        //   AttackType chosenAttack = (AttackType)Random.Range(0, System.Enum.GetValues(typeof(AttackType)).Length);
        
        //AttackType chosenAttack = AttackType.SpawnObsticles;
        _currentState = AIState.Attacking;

        //Execute the chosen attack
        switch (_index)
        {
            case 0:
                StartCoroutine(SpiralShootingSequence());
                break;
            case 1:
                StartCoroutine(ZoneExplosionSequence());
                break;
            case 2:
                StartCoroutine(LaserSpinningSequence());
                break;
            case 3:
                StartCoroutine(BulletShowerSequence());
                break;
            case 4:
                StartCoroutine(GroundPoundSequence());
                break;
            case 5:
                StartCoroutine(SpiralShootingSequence());
                break;
            case 6:
                StartCoroutine(BulletShowerSequence());
                break;
            case 7:
                StartCoroutine(LaserSpinningSequence());
                break;
            case 8:
                StartCoroutine(GroundPoundSequence());
                break;
            case 9:
                StartCoroutine(BulletShowerSequence());
                break;
            case 10:
                StartCoroutine(ZoneExplosionSequence());
                break;
            case 11:
                StartCoroutine(SpiralShootingSequence());
                break;
            case 12:
                StartCoroutine(ZoneExplosionSequence());
                break;
            case 13:
                StartCoroutine(LaserSpinningSequence());
                break;
            case 14:
                StartCoroutine(BulletShowerSequence());
                break;
            case 15:
                StartCoroutine(GroundPoundSequence());
                break;
            case 16:
                StartCoroutine(SpiralShootingSequence());
                break;
            case 17:
                StartCoroutine(CircleTravellingProjectileSequence());
                break;
            case 18:
                StartCoroutine(LaserSpinningSequence());
                break;
            case 19:
                StartCoroutine(GroundPoundSequence());
                break;
            case 20:
                StartCoroutine(BulletShowerSequence());
                break;
            case 21:
                StartCoroutine(ZoneExplosionSequence());
                break;
            case 22:
                StartCoroutine(WaitingTime());      //Health Bar Stop Decreasing
                break;
            case 23:                                //Transition
                StartCoroutine(OneShotSequence());
                break;
            case 24:
                StartCoroutine(WaitingTime());
                break;
            case 25:
                StartCoroutine(SpiralShootingSequence()); //Health Bar Decreasing again
                break;
            case 26:
                StartCoroutine(SpawnObsticleSequence());
                StartCoroutine(BulletShowerSequence());
                break;
            case 27:
                StartCoroutine(LaserSpinningSequence());
                StartCoroutine(GroundPoundSequence());
                break;
            case 28:
                StartCoroutine(SpiralShootingSequence());
                break;
            case 29:
                StartCoroutine(SpiralShootingReverseSequence());
                break;
            case 30:
                StartCoroutine(ZoneExplosionSequence());
                StartCoroutine(WaitingTime());
                StartCoroutine(ZoneExplosionSequence());
                break;
            case 31:
                StartCoroutine(SpawnObsticleSequence());
                StartCoroutine(GroundPoundSequence());
                StartCoroutine(BulletShowerSequence());
                break;
            case 32:
                StartCoroutine(LaserSpinningSequence());
                StartCoroutine(SpiralShootingSequence());
                StartCoroutine(SpiralShootingReverseSequence());
                break;
                //Add other cases for different attacks

        }
        _index++;
    }

    IEnumerator WaitingTime()
    {
        yield return new WaitForSeconds(5.0f);
        // Restart the AI behavior routine
        yield return null;
    }

    IEnumerator WaitingShortTime()
    {
        yield return new WaitForSeconds(1.0f);
        // Restart the AI behavior routine
        yield return null;
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
