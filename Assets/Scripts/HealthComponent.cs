using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public float MaxHealth = 100.0f;
    public UnityEvent OnDeath;

    public float CurrentHealth => _currentHealth;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = MaxHealth;
    }

    private void Start()
    {
        _currentHealth = MaxHealth;
    }

    public void Damage(float amount, GameObject Instigator)
    {
        _currentHealth -= amount;
        Mathf.Clamp(_currentHealth, 0, MaxHealth);
        if (_currentHealth == 0)
        {
            OnDeath.Invoke();
           // Debug.Log($"{gameObject.name}: Dead");
        }

        Debug.Log($"{gameObject.name}: Current Health: {_currentHealth}");
    }

    public void Heal(float amount, GameObject Healer)
    {
        _currentHealth += amount;
        Mathf.Clamp(_currentHealth, 0, MaxHealth);

    }
}
