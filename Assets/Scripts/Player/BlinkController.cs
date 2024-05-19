using UnityEngine;

public class BlinkController : MonoBehaviour
{
    public Material blinkMaterial;
    public float blinkDuration = 2.0f;
    public int numberOfBlinks = 5;

    private float blinkInterval;
    private bool isBlinking = false;
    private float blinkTime = 0f;

    void Start()
    {
        if (blinkMaterial == null)
        {
            Debug.LogError("Blink material is not assigned.");
            return;
        }

        blinkInterval = blinkDuration / (numberOfBlinks * 2);
    }

    void Update()
    {
        if (isBlinking)
        {
            blinkTime += Time.deltaTime;

            if (blinkTime >= blinkDuration)
            {
                isBlinking = false;
                blinkMaterial.SetFloat("_BlinkAlpha", 1.0f);
                return;
            }

            float blinkAlpha = Mathf.PingPong(blinkTime, blinkInterval * 2) < blinkInterval ? 0.0f : 1.0f;
            blinkMaterial.SetFloat("_BlinkAlpha", blinkAlpha);
        }
    }

    public void StartBlinking()
    {
        isBlinking = true;
        blinkTime = 0f;
    }
}
