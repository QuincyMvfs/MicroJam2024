using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecreasingBar : MonoBehaviour
{
    public Image barImage; // Reference to the Image component
    public float duration = 10f; // Time in seconds for the bar to go from full to empty

    private float elapsedTime = 0f;
    private bool isDecreasing = false;

    void Start()
    {
        if (barImage == null)
        {
            barImage = GetComponent<Image>();
        }
        StartDecreasing();
    }

    void Update()
    {
        if (isDecreasing)
        {
            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the fill amount
            float fillAmount = Mathf.Clamp01(1 - (elapsedTime / duration));

            // Set the fill amount
            barImage.fillAmount = fillAmount;

            // Optional: Stop updating after the duration is reached
            if (elapsedTime >= duration)
            {
                StopDecreasing();
            }
        }
    }

    public void StartDecreasing()
    {
        isDecreasing = true;
    }

    public void StopDecreasing()
    {
        isDecreasing = false;
    }

    public void RestartDecreasing(float delay)
    {
        StopAllCoroutines();
        StartCoroutine(RestartAfterDelay(delay));
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        elapsedTime = 0f; // Reset elapsed time
        StartDecreasing();
    }
}
