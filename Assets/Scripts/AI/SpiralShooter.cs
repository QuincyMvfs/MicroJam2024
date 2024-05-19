using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralShooter : MonoBehaviour
{
    [Header("Spinning Variables")]
    [SerializeField] private float _spinSpeed = 25.0f;
    [SerializeField] private float _attackDuration = 5.0f;

    [Header("Projectile Variables")]
    [SerializeField] private float _fireDelay = 0.05f;
    [SerializeField] private float _projectileSpeed = 1000;
    [SerializeField] private float _projectileLifetime = 3.0f;
    [SerializeField] private float _damage = 10.0f;
    [SerializeField] private int _totalProjectilePerFire = 4;

    [Header("Components")]
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private AudioSource _shootSFX;

    private float _timeToStopFiring = 0;
    private float _timeToReverseFiring = 4.5f;
    private AIController _aiController;

    private void Awake()
    {
        _aiController = GetComponent<AIController>();
    }

    public void StartSpinning()
    {
        _timeToStopFiring = Time.time + _attackDuration;
        StartCoroutine(SpinningAttack());
        StartCoroutine(Spinning());
    }

    public void StartSpinningReverse()
    {
        _timeToStopFiring = Time.time + _attackDuration;
        StartCoroutine(SpinningAttack());
        StartCoroutine(SpinningReverse());
    }

    private IEnumerator SpinningAttack()
    {
        while (_timeToStopFiring > Time.time)
        {
            AudioSource SpawnedAudio = Instantiate(_shootSFX, _muzzleTransform.position, _muzzleTransform.rotation);
            Destroy(SpawnedAudio, 0.5f);

            float degreeDifference = 360 / _totalProjectilePerFire;
            float currentDegree = 0 - degreeDifference;
            for (int i = 0; i < _totalProjectilePerFire; i++)
            {
                currentDegree += degreeDifference;
                Projectile Spawned = Instantiate(_projectile, _muzzleTransform.position, _muzzleTransform.rotation);
                Spawned.transform.Rotate(0, currentDegree, 0);
                Spawned.Launch(_projectileSpeed, _damage, _projectileLifetime, this.gameObject);
            }
          
            yield return new WaitForSeconds(_fireDelay);
        }
    }

    private IEnumerator Spinning()
    {
        while (_timeToStopFiring - _timeToReverseFiring> Time.time)
        {
            _muzzleTransform.Rotate(transform.position, _spinSpeed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(SpinningReverse());
    }

    private IEnumerator SpinningReverse()
    {
        while (_timeToStopFiring > Time.time)
        {
            _muzzleTransform.Rotate(transform.position, -_spinSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
