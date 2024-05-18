using System.Collections;
using UnityEngine;

public class OneShotCoverAttack : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 999999.0f;
    [SerializeField] private GameObject _vfxPrefab;
    [SerializeField] private float _warningTimer = 10.0f;
    [SerializeField] private GameObject _shaderObject;
    private AIController _aiController;
    private Transform _playerTransform;

    private Material _shaderMaterial;

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

        PlayerController Player = FindObjectOfType<PlayerController>();
        if (Player != null)
        {
            _playerTransform = Player.gameObject.transform;
        }
    }

    public void ExecuteOneShot()
    {
        _shaderMaterial.SetFloat("_FillAmount", 1.0f);
        StartCoroutine(PerformOneShot());
    }

    IEnumerator PerformOneShot()
    {
        // Wait for the warning timer duration before executing the attack
        yield return new WaitForSeconds(_warningTimer);

        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5000000))
        {
            HealthComponent health = hit.collider.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.Damage(_damageAmount, this.gameObject);
            }

            // Draw the ray in the Scene view for debugging purposes
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 5000000, Color.red, 1f);
        }

        // Spawn the VFX at the current position
        if (_vfxPrefab != null)
        {
            Instantiate(_vfxPrefab, this.transform.position, Quaternion.identity);
        }
        _shaderMaterial.SetFloat("_FillAmount", 0.0f);
        _aiController.ResetToIdle();
    }
}
