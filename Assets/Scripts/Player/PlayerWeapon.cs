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
    [SerializeField] private Transform _muzzleTransform;

    private CharacterMovement _characterMovement;

    private void Start()
    {
        StartCoroutine(ShootLoop());
    }

    private IEnumerator ShootLoop()
    {
        while (true)
        {
            Projectile SpawnedProjectile = Instantiate(_projectile, _muzzleTransform.position, _muzzleTransform.rotation);
            SpawnedProjectile.Launch(_projectileSpeed, _projectileDamage, _projectileLifetime, this.gameObject);
            yield return new WaitForSeconds(_shootDelay);
        }
    }

}
