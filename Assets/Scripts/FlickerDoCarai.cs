using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickerDoCarai : MonoBehaviour
{
        private Light LightToFlicker;
    [SerializeField, Range(0f, 300f)] private float minIntensity = 0.5f; 
    [SerializeField, Range(0f, 300f)] private float maxIntensity = 1.2f;
    [SerializeField, Min(0f)] private float TimeBetweenIntensity = 0.1f;

    private float currentTimer;

    private void Awake()
    {
        if (LightToFlicker == null)
        {
            LightToFlicker = GetComponent<Light>();
        }

        ValidateIntensityBounds();

    }

    private void Update()
    {
        currentTimer += Time.deltaTime;

        if (currentTimer >= TimeBetweenIntensity)
        {
            LightToFlicker.intensity = Random.Range(minIntensity, maxIntensity);
            currentTimer = 0;
        }
    
    }

    private void ValidateIntensityBounds()
    {
        if (!(minIntensity > maxIntensity))
        {
           return;
        }
        Debug.LogWarning("Vai Se foder nao ta funcionando");
        (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
    }
}