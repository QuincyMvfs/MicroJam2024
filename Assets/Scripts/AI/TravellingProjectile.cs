using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class CircleTravellingProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float _launchSpeed;
    [SerializeField] private float _circularMovementSpeed;
    [SerializeField] private float _damage = 45.0f;
    [SerializeField] private float[] perLaneLifetime;

    private float _lifetime = 10f;

    [Header("Components")]
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _muzzleTransform;

    private AIController _aiController;
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

        int chosenLane = Random.Range(0, 3);
        float randomizedLifetime = perLaneLifetime[chosenLane];
        //choose lane
        //spawn projectile
        Projectile spawnedProj = Instantiate(_projectile, _muzzleTransform.position, _muzzleTransform.rotation);
        //launch projectile in straight line towards player
        _lifetime = randomizedLifetime;
        spawnedProj.Launch(_launchSpeed, _damage, _lifetime, this.gameObject);
        //destroy first projectile by setting lifetime and spawn a new one that travels in a circle
       
        //Projectile secondSpawnedProj = Instantiate,
        //rotate 90 degrees when arriving at lane
        
        //travel along circle path
        //destroy after x time
    }  
    private IEnumerator SpawnSecondProjectile()
    {
        yield return new WaitForSeconds(_lifetime);
    }
}
