using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class CircleTravellingProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float _launchSpeed;
    [SerializeField] private float _circularMovementSpeed;
    [SerializeField] private float _damage = 45.0f;
    [SerializeField] private float _circularProjectileLifetime;
    [SerializeField] private float[] perLaneLifetime;
    [SerializeField] private Quaternion _rotationOffset;
    [SerializeField] private float _delayReturnToIdle = 1.0f;

    private float _lifetime = 10f;

    [Header("Components")]
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private Transform[] _zoneTransforms; 

    private AIController _aiController;
    private Projectile spawnedProjectile;
    // Start is called before the first frame update
    private void Awake()
    {
        _aiController = GetComponent<AIController>();
    }

    public void LaunchCircleTravellingProjectile()
    {
        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        //choose lane
        int chosenLane = Random.Range(0, 3);
        Transform randomizedLane = _zoneTransforms[chosenLane];
        Debug.Log("Launching, Lane" + (1 + chosenLane).ToString());
        //spawn projectile
        spawnedProjectile = Instantiate(_projectile, _muzzleTransform.position, _muzzleTransform.rotation);
        //launch projectile in straight line towards player
        spawnedProjectile.Launch(0, _damage, _lifetime, this.gameObject);
        StartCoroutine(LaunchProjectileForwards(spawnedProjectile, randomizedLane));
        //destroy first projectile by setting lifetime and spawn a new one that travels in a circle
        StartCoroutine(DelaySecondProjectile());
        //Projectile secondSpawnedProj = Instantiate,
        //rotate 90 degrees when arriving at lane
        //travel along circle path
        //destroy after x time
    }  
    private IEnumerator LaunchProjectileForwards(Projectile projectile, Transform destination)
    {
        while (projectile.transform.position != destination.position)
        {
            projectile.transform.position = Vector3.Lerp(transform.position, destination.position, _lifetime);
        }
        yield return null;
    }
    private IEnumerator DelaySecondProjectile()
    {
        yield return new WaitForSeconds(_lifetime - 1f);
        SpawnSecondProjectile();
        StopCoroutine(DelaySecondProjectile());
    }

    private IEnumerator DelayReturnToIdle()
    {
        yield return new WaitForSeconds(_delayReturnToIdle);
        _aiController.ResetToIdle();
        StopCoroutine(DelayReturnToIdle());
    }
    private void SpawnSecondProjectile()
    {
        if (spawnedProjectile != null)
        {
            Quaternion rotationresult = spawnedProjectile.transform.rotation * _rotationOffset;
            Projectile secondSpawnedProj = Instantiate(_projectile, spawnedProjectile.transform.position, rotationresult);
            secondSpawnedProj.Launch(_circularMovementSpeed, _damage, _circularProjectileLifetime, this.gameObject);
        }
    }
}
