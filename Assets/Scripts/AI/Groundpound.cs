using UnityEngine;
using System.Collections;

public class Groundpound : MonoBehaviour
{
    [SerializeField] private float _maxRadius = 20f;
    [SerializeField] private float _explosionDuration = 2f; 
    [SerializeField] private float _damageAmount = 1f;
    [SerializeField] private bool _drawGizmos = false;
    [SerializeField] private GameObject _floor;
    [SerializeField] private GameObject _shaderObject;

    [SerializeField] private AudioSource _groundPoundWarningSFX;

    private AIController _aiController;

    private Material _shaderMaterial;
    private bool _hasExploded = false;
    private float _cylinderRadius;

    private void Start()
    {
        _aiController = GetComponent<AIController>();

        if (_shaderObject != null)
        {
            Renderer renderer = _shaderObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Get the material instance
                _shaderMaterial = renderer.material;
            }
        }

        // Calculate the radius of the cylinder's top
        _cylinderRadius = _floor.transform.localScale.x / 2f;
    }

    public void StartExplosion()
    {
        if (!_hasExploded)
        {
            // SPAWN SFX
            if (_groundPoundWarningSFX != null)
            {
                AudioSource SpawnedAudio = Instantiate(_groundPoundWarningSFX, transform.position, transform.rotation);
                Destroy(SpawnedAudio, 2f);
            }

            _hasExploded = true;
            StartCoroutine(LerpExplosionRadius());
        }
    }

    IEnumerator LerpExplosionRadius()
    {
        float currentRadius = 0f;
        float startTime = Time.time;
        float endTime = startTime + _explosionDuration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / _explosionDuration;
            currentRadius = Mathf.Lerp(0f, _maxRadius, t);
            float fillAmount = currentRadius / _cylinderRadius;

            if (_shaderMaterial != null)
            {
                // Update the shader's fill amount
                _shaderMaterial.SetFloat("_FillAmount", fillAmount);
            }

            // Trigger the explosion effect at the current radius
            TriggerExplosionAtRadius(currentRadius);
            yield return null; // Wait until the next frame
        }

        // Ensure the final state is set correctly
        if (_shaderMaterial != null)
        {
            _shaderMaterial.SetFloat("_FillAmount", _maxRadius / _cylinderRadius); // Ensure the fill amount is set to 1 at the end
        }

        // Trigger the final explosion effect at the maximum radius
        TriggerExplosionAtRadius(_maxRadius);

        // Reset the explosion effect after completion
        ResetExplosion();
    }

    void TriggerExplosionAtRadius(float radius)
    {
        // Apply force to players within this radius and damage
        Collider[] hitColliders = Physics.OverlapSphere(_floor.transform.position, radius);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == this.gameObject) continue;

            CharacterMovement character = hitCollider.GetComponent<CharacterMovement>();
            if (character != null)
            {
                character.PushToLastLane();
            }

            HealthComponent targetHealth = hitCollider.GetComponent<HealthComponent>();
            if (targetHealth != null)
            {
                targetHealth.Damage(_damageAmount, this.gameObject);
            }
        }
    }

    private void ResetExplosion()
    {
        // Reset the hasExploded flag to allow re-triggering the explosion
        _hasExploded = false;

        // Reset the shader's fill amount to 0
        if (_shaderMaterial != null)
        {
            _shaderMaterial.SetFloat("_FillAmount", 0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (_drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawWireSphere(_floor.transform.position, _maxRadius);
        }
    }
}
