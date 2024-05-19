using System.Collections;
using UnityEngine;

public class LaserScaler : MonoBehaviour
{
    [SerializeField] private float _growDuration = 2.0f; 
    
    private Transform _laserMesh;
    private Vector3 _targetScale = new Vector3(0.2f, 30, 0.2f); 
    private Vector3 _targetPosition = new Vector3(0, 0, 30); 
    private Vector3 _initialScale;
    private Vector3 _initialPosition;

    private void Start()
    {
        if (_laserMesh == null)
        {
            _laserMesh = transform;
        }

        _initialScale = Vector3.zero;
        _initialPosition = Vector3.zero;

        _targetScale = _laserMesh.localScale;
        _targetPosition = _laserMesh.localPosition;

        _laserMesh.localScale = Vector3.zero;
        _laserMesh.localPosition = Vector3.zero;

        StartCoroutine(GrowLaser());
    }

    private IEnumerator GrowLaser()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _growDuration)
        {
            elapsedTime += Time.deltaTime;
            float time = elapsedTime / _growDuration;

            _laserMesh.localScale = Vector3.Lerp(_initialScale, _targetScale, time);
            _laserMesh.localPosition = Vector3.Lerp(_initialPosition, _targetPosition, time);

            yield return null;
        }

        _laserMesh.localScale = _targetScale;
        _laserMesh.localPosition = _targetPosition;
    }
}
