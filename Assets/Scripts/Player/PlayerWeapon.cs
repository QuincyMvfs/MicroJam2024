using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]


public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Properties")] 
    [SerializeField] private float _projectileSpeed = 1000.0f;
    [SerializeField] private float _projectileDamage = 10.0f;
    [SerializeField] private float _projectileLifetime = 3.0f;
    [SerializeField] private float _shootDelay = 1f;

    [Header("Components")]
    [SerializeField] private Projectile _projectile;
    [SerializeField] private GameObject _muzzleFlashVFX;
    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private AudioSource _playerShootingSFX;

    private Transform _targetTransform;
    private CharacterMovement _characterMovement;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void Start()
    {
        _targetTransform = _characterMovement.LookTarget.transform;
        StartCoroutine(ShootLoop());
    }

    private IEnumerator ShootLoop()
    {
        while (true)
        {
            Vector3 Direction = (_targetTransform.position - _muzzleTransform.position).normalized;
            Quaternion LookRotation = Quaternion.LookRotation(Direction);
            Projectile SpawnedProjectile = Instantiate(_projectile, _muzzleTransform.position, LookRotation);
            GameObject SpawnedMuzzleFlash = Instantiate(_muzzleFlashVFX, _muzzleTransform.position, LookRotation);
            SpawnedMuzzleFlash.transform.parent = _muzzleTransform;
            Destroy(SpawnedMuzzleFlash, 1f);
            SpawnedProjectile.Launch(_projectileSpeed, _projectileDamage, _projectileLifetime, this.gameObject);

            if (_playerShootingSFX != null)
            {
                AudioSource SpawnedAudio = Instantiate(_playerShootingSFX, transform.position, transform.rotation);
                Destroy(SpawnedAudio, 0.5f);
            }
            yield return new WaitForSeconds(_shootDelay);
        }
    }

}
