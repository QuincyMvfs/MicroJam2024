using UnityEngine;

public class OneShotCoverAttack : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 999999999.0f;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject _vfxPrefab;

    public void ExecuteOneShot()
    {
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

        if (_vfxPrefab != null)
        {
            Instantiate(_vfxPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
