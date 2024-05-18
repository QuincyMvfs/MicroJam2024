using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]


public class Projectile : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _collider;
    private float _damage;
    private GameObject _owner;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    public void Launch(float speed, float damage, GameObject owner)
    {
        _rb.AddForce(transform.forward * speed);
        _damage = damage;
        _owner = owner;
        Destroy(gameObject, 10.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _owner) return;

        if (other.TryGetComponent<HealthComponent>(out HealthComponent health))
        {
            health.Damage(_damage, this.gameObject);
            Destroy(this.gameObject);
        }
    }


}
