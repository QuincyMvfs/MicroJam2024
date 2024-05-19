using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class HealthComponent : MonoBehaviour
{
    [Header("Health Variables")]
    [SerializeField] public float MaxHealth = 100.0f;
    [SerializeField] private float _godFrames = 1f;

    [Header("SFX")]
    [SerializeField] private AudioSource[] _hitSounds;
    [SerializeField] private AudioSource[] _deathSounds;

    [Header("VFX")]
    [SerializeField] private VisualEffect _hitVFX;

    [Header("Unity Events")]
    public UnityEvent OnDeath;

    private bool _isDead = false;
    private float _nextHitTime;

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
        if(_isDead) return;

        if (_nextHitTime < Time.time)
        {
            if (_hitVFX != null)
            {
                if (_hitVFX.gameObject.activeInHierarchy) { _hitVFX.Play(); }
                else { _hitVFX.gameObject.SetActive(true); }
            }

            _nextHitTime = Time.time + _godFrames;
            _currentHealth -= amount;
            Mathf.Clamp(_currentHealth, 0, MaxHealth);
            if (_currentHealth <= 0)
            {
                AudioSource deathSound = GetSound(_deathSounds);
                if (deathSound != null)
                {
                    AudioSource SpawnedAudio = Instantiate(deathSound, transform.position, transform.rotation);
                    Destroy(SpawnedAudio, 2f);
                }

                _isDead = true;
                OnDeath.Invoke();
                Debug.Log($"{gameObject.name}: Dead");
            }
            else
            {
                AudioSource hitSound = GetSound(_hitSounds);
                if (hitSound != null)
                {
                    AudioSource SpawnedAudio = Instantiate(hitSound, transform.position, transform.rotation);
                    Destroy(SpawnedAudio, 2f);
                }
            }

            // Prevents getting spammed from boss
            if (this.gameObject.name != "Boss") Debug.Log($"{gameObject.name}: Current Health: {_currentHealth}");
        }
      
    }

    public void Heal(float amount, GameObject Healer)
    {
        _currentHealth += amount;
        Mathf.Clamp(_currentHealth, 0, MaxHealth);

    }

    private AudioSource GetSound(AudioSource[] Sounds)
    {
        if (Sounds == null || Sounds.Length == 0)
        {
            if (this.gameObject.name != "Boss") Debug.Log($"Null Hitsound");
            return null;
        }

        int RandomInt = Random.Range(0, Sounds.Length);
        return Sounds[RandomInt];
    }
}
