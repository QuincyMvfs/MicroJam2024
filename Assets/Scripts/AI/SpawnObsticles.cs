using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObsticles : MonoBehaviour
{
    [SerializeField] private GameObject _obsticleToSpawn;
    [SerializeField] private float _obsticleLifetime = 10.0f;
    [SerializeField] private float _angleDistanceFromPlayer = 45.0f;
    [SerializeField] private float _obsticleMoveSpeed = 10.0f;
    [SerializeField] private float _distanceFromBoss = 5.0f;

    private GameObject[] _obsticles = new GameObject[2];
    private Transform _playerTransform;
    private float _spawnYOffset = 10;
    private Vector3 _leftStartPosition;
    private Vector3 _rightStartPosition;
    private bool _isMoving = false;

    private void Awake()
    {
        PlayerController Player = FindObjectOfType<PlayerController>();
        if (Player != null)
        {
            _playerTransform = Player.gameObject.transform;
        }
    }

    public void StartSpawningObsticles()
    {
        if (_isMoving) return;

        // Rotations
        Quaternion LeftRotation = transform.rotation;
        Quaternion axisLeftRotation = Quaternion.AngleAxis(-_angleDistanceFromPlayer, Vector3.up);
        LeftRotation *= axisLeftRotation;
        Quaternion RightRotation = transform.rotation;
        Quaternion axisRightRotation = Quaternion.AngleAxis(_angleDistanceFromPlayer, Vector3.up);
        RightRotation *= axisRightRotation;

        Vector3 Direction = (_playerTransform.position - transform.position).normalized;

        // First Obsticle
        Vector3 RotatedDir = LeftRotation * Direction;
        Vector3 SpawnPosition = transform.position + (RotatedDir * _distanceFromBoss);
        _leftStartPosition = SpawnPosition;
        SpawnPosition.y -= _spawnYOffset;
        LeftRotation = Quaternion.LookRotation(RotatedDir);
        _obsticles[0] = Instantiate(_obsticleToSpawn, SpawnPosition, LeftRotation);

        // Second Obsticle
        RotatedDir = RightRotation * Direction;
        SpawnPosition = transform.position + (RotatedDir * _distanceFromBoss);
        _rightStartPosition = SpawnPosition;
        SpawnPosition.y -= _spawnYOffset;
        RightRotation = Quaternion.LookRotation(RotatedDir);
        _obsticles[1] = Instantiate(_obsticleToSpawn, SpawnPosition, RightRotation);

        StartCoroutine(SummonObsticleLerp());
    }

    private IEnumerator SummonObsticleLerp()
    {
        _isMoving = true;
        // Lerp up to block the player
        while (Vector3.Distance(_obsticles[0].transform.position, _leftStartPosition) > 0.01f)
        {
            _obsticles[0].transform.position = Vector3.MoveTowards(_obsticles[0].transform.position, _leftStartPosition, _obsticleMoveSpeed * Time.deltaTime);
            _obsticles[1].transform.position = Vector3.MoveTowards(_obsticles[1].transform.position, _rightStartPosition, _obsticleMoveSpeed * Time.deltaTime);
            Debug.Log("moving");
            yield return null;
        }

        yield return new WaitForSeconds(_obsticleLifetime);
        StartCoroutine(RemoveObsticleLerp());
    }

    private IEnumerator RemoveObsticleLerp()
    {
        // Lerp down
        Vector3 leftEndPosition = new Vector3(_leftStartPosition.x, _leftStartPosition.y - _spawnYOffset, _leftStartPosition.z);
        Vector3 rightEndPosition = new Vector3(_rightStartPosition.x, _rightStartPosition.y - _spawnYOffset, _rightStartPosition.z);
        while (Vector3.Distance(_obsticles[0].transform.position, leftEndPosition) > 0.01f)
        {
            _obsticles[0].transform.position = Vector3.MoveTowards(_obsticles[0].transform.position, leftEndPosition, _obsticleMoveSpeed * Time.deltaTime);
            _obsticles[1].transform.position = Vector3.MoveTowards(_obsticles[1].transform.position, rightEndPosition, _obsticleMoveSpeed * Time.deltaTime);
            Debug.Log("done");
            yield return null;
        }

        Destroy(_obsticles[0]);
        Destroy(_obsticles[1]);
        _isMoving = false;
    }
}
