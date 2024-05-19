using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ExplodingPart : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        _rb.useGravity = true;
    }

    public void ExplodePart(float force, Vector3 explosionPosition, float explosionRadius)
    {
        _rb.isKinematic = false;
        _rb.AddExplosionForce(force, explosionPosition, explosionRadius);
    }
}
