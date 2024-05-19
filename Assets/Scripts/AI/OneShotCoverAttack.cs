using System.Collections;
using UnityEngine;

public class OneShotCoverAttack : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 999999.0f;
    [SerializeField] private GameObject _vfxPrefab;
    [SerializeField] private float _warningTimer = 10.0f;
    [SerializeField] private GameObject _shaderObject;
    [SerializeField] private Transform[] _coverObjects; // Array of cover objects
    [SerializeField] private float _coverMoveDuration = 1.0f; // Duration for cover objects to move up
    [SerializeField] private float _coverMoveHeight = 3.0f;

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
        StartCoroutine(PerformOneShot());
    }

    IEnumerator PerformOneShot()
    {
        _shaderMaterial.SetFloat("_FillAmount", 1.0f);

        StartCoroutine(MoveCoverObjects(Vector3.up * _coverMoveHeight));

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
        }

        // Spawn the VFX at the current position
        if (_vfxPrefab != null)
        {
            Instantiate(_vfxPrefab, this.transform.position, Quaternion.identity);
        }

        StartCoroutine(MoveCoverObjects(Vector3.down * _coverMoveHeight));
        _shaderMaterial.SetFloat("_FillAmount", 0.0f);
        _aiController.ResetToIdle();
    }

    IEnumerator MoveCoverObjects(Vector3 direction)
    {
        float elapsedTime = 0.0f;
        Vector3[] initialPositions = new Vector3[_coverObjects.Length];

        // Store initial positions of cover objects
        for (int i = 0; i < _coverObjects.Length; i++)
        {
            initialPositions[i] = _coverObjects[i].position;
        }

        while (elapsedTime < _coverMoveDuration)
        {
            for (int i = 0; i < _coverObjects.Length; i++)
            {
                _coverObjects[i].position = Vector3.Lerp(initialPositions[i], initialPositions[i] + direction, elapsedTime / _coverMoveDuration);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final positions are set accurately
        for (int i = 0; i < _coverObjects.Length; i++)
        {
            _coverObjects[i].position = initialPositions[i] + direction;
        }
    }
}
