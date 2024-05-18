using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningLasers : MonoBehaviour
{
    [Header("Spinning Laser Variables")]
    [SerializeField] private float _spinSpeed = 25.0f;
    [SerializeField] private float _attackDuration = 5.0f;
    [SerializeField] private float _damage = 10.0f;
    [SerializeField] private int _totalLasers = 4;

    [Header("Components")]
    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private GameObject _laserPrefab;

    private float _timeToStopFiring = 0;
    private AIController _aiController;

    private void Awake()
    {
        _aiController = GetComponent<AIController>();
    }

    public void StartSpinningLasers()
    {
        _timeToStopFiring = Time.time + _attackDuration;
        StartCoroutine(Spinning());
    }

    private IEnumerator Spinning()
    {
        float degreeDifference = 360 / _totalLasers;
        float currentDegree = 0 - degreeDifference;
        for (int i = 0; i < _totalLasers; i++)
        {
            currentDegree += degreeDifference;
            Quaternion spawnRotation = Quaternion.Euler(0, currentDegree, 0);
            GameObject SpawnedLaser = Instantiate(_laserPrefab, _muzzleTransform.position, spawnRotation);
            Destroy(SpawnedLaser, _attackDuration);
            SpawnedLaser.transform.parent = _muzzleTransform.transform;
        }

        while (_timeToStopFiring > Time.time)
        {
            _muzzleTransform.Rotate(transform.position, _spinSpeed * Time.deltaTime);
            yield return null;
        }

        _aiController.ResetToIdle();
    }
}
