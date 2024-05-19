using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingPartController : MonoBehaviour
{
    [SerializeField] private float _explosionForce = 500.0f;
    [SerializeField] private float _explosionRadius = 2.0f;
    [SerializeField] private ExplodingPart[] _explodingParts;
    [SerializeField] private GameObject _coreGameObject;
    [SerializeField] private AudioSource _explosionsSFX;
    [SerializeField] private GameObject _explosionsVFX;

    private Vector3 _corePos;
    private Quaternion _coreRot;

    private void Start()
    {
        _corePos = _coreGameObject.transform.position;
        _coreRot = _coreGameObject.transform.rotation;
    }

    public void ExplodeParts()
    {
        if (_explosionsSFX != null) Instantiate(_explosionsSFX, _corePos, _coreRot);
        if (_explosionsVFX != null) Instantiate(_explosionsVFX, _corePos, _coreRot);

        foreach (ExplodingPart part in _explodingParts)
        {
            part.ExplodePart(_explosionForce, _corePos, _explosionRadius);
        }

        Destroy(_coreGameObject);
    }
}
