using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]

public class TriggerVolume : MonoBehaviour
{
    [Header("Damage/ Heal Values")]
    [SerializeField] private float DamageAmount = 5.0f;
    [SerializeField] private float HealAmount = 5.0f;

    [Header("Unity Events")]
    public UnityEvent<Collider> OnEnter;
    public UnityEvent<Collider> OnExit;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out PlayerController controller)) return;
        OnEnter.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out PlayerController controller)) return;
        OnExit.Invoke(other);
    }

    public void DamageOther(Collider other)
    {
        if (other.TryGetComponent<HealthComponent>(out HealthComponent health))
        {
            health.Damage(DamageAmount, this.gameObject);
        }
    }

    public void HealOther(Collider other)
    {
        if (other.TryGetComponent<HealthComponent>(out HealthComponent health))
        {
            health.Heal(HealAmount, this.gameObject);
        }
    }
}
