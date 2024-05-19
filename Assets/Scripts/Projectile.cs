using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]


public class Projectile : MonoBehaviour
{
    [SerializeField] private AudioSource _hitObsticleSFX;
    [SerializeField] private AudioSource _hitHealthSFX;


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

    public void Launch(float speed, float damage, float lifetime, GameObject owner)
    {
        _rb.AddForce(transform.forward * speed);
        _damage = damage;
        _owner = owner;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _owner) return;

        if (other.TryGetComponent<HealthComponent>(out HealthComponent health))
        {
            health.Damage(_damage, this.gameObject);
            if (_hitHealthSFX != null)
            {
                AudioSource SpawnedAudio = Instantiate(_hitHealthSFX, transform.position, transform.rotation);
                Destroy(SpawnedAudio, 0.5f);
            }
            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            if (_hitObsticleSFX != null)
            {
                AudioSource SpawnedAudio = Instantiate(_hitObsticleSFX, transform.position, transform.rotation);
                Destroy(SpawnedAudio, 0.5f);
            }
            Destroy(this.gameObject);
        }
    }


}
