using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class CircleTravellingProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float _launchSpeed = 100f;
    [SerializeField] private float _rotationDegreesPerSecond = 5f;
    [SerializeField] private float _damage = 45.0f;
    [SerializeField] private float _lifetime = 20f;

    [Header("Components")]
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _launchTransform;
    [SerializeField] private Transform _rotationPivot;
    [SerializeField] private Transform[] _zoneTransforms; 

    private float _delayReturnToIdle = 1.0f;
    private AIController _aiController;
    private Projectile _spawnedProjectile;
    private Quaternion _defaultRotation;
    // Start is called before the first frame update
    private void Awake()
    {
        _aiController = GetComponent<AIController>();
        _defaultRotation = _launchTransform.rotation;
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
        //Debug.Log("Launching, Lane" + (1 + chosenLane).ToString());
        
        //spawn projectile
        _spawnedProjectile = Instantiate(_projectile, _launchTransform.position, _launchTransform.rotation, _rotationPivot);
        
        //launch projectile in straight line towards player
        _spawnedProjectile.Launch(0, _damage, _lifetime, this.gameObject);
        StartCoroutine(LaunchProjectileForwards(_spawnedProjectile, randomizedLane));
    }
    
    private IEnumerator LaunchProjectileForwards(Projectile projectile, Transform destination)
    {
        if (projectile != null)
        {
            Transform startPoint = projectile.transform;
            while (projectile.transform.position != destination.position)
            {
                float t = projectile.transform.position.z / destination.position.z;
                projectile.transform.position = Vector3.Lerp(startPoint.position, destination.position, t);
                yield return new WaitForSeconds(1.0f / (_launchSpeed));
            }
        }

        yield return null;
        
        StartCoroutine(RotateProjectile());
    }

    private IEnumerator RotateProjectile()
    {
        while (true)
        {
            if (_spawnedProjectile == null)
            {
                StartCoroutine(DelayReturnToIdle());
                yield return null;
            }
            else
            {
                Vector3 rotation = new Vector3(0, _rotationDegreesPerSecond, 0);
                _rotationPivot.Rotate(rotation);
                yield return new WaitForSeconds(0.01f);
            }
        }
        
    }
    private IEnumerator DelayReturnToIdle()
    {
        yield return new WaitForSeconds(_delayReturnToIdle);
        _aiController.ResetToIdle();
        _launchTransform.rotation = _defaultRotation;
        StopCoroutine(DelayReturnToIdle());
    }
}
