using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    // Rotation speed for each axis
    public float rotationSpeedX = 10f;
    public float rotationSpeedY = 10f;
    public float rotationSpeedZ = 10f;

    // Random rotation directions
    private Vector3 randomRotationDirection;

    void Start()
    {
        // Initialize with random directions for each axis
        randomRotationDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
    }

    void Update()
    {
        // Calculate rotation for this frame
        Vector3 rotationThisFrame = new Vector3(
            randomRotationDirection.x * rotationSpeedX * Time.deltaTime,
            randomRotationDirection.y * rotationSpeedY * Time.deltaTime,
            randomRotationDirection.z * rotationSpeedZ * Time.deltaTime
        );

        // Apply rotation
        transform.Rotate(rotationThisFrame);
    }
}
