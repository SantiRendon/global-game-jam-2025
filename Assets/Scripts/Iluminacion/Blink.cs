using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Blink : MonoBehaviour
{
    public Light2D lightSource;
    public float initialIntensity = 1.0f; // Intensidad estable de la luz
    public float flickerIntensity = 0.2f; // Intensidad durante el parpadeo
    public float flickerDuration = 0.1f; // Duración del parpadeo
    public int min = 1;
    public int max = 10;

    private void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light2D>();
        }

        // Aseguramos que la luz comience con la intensidad inicial
        lightSource.intensity = initialIntensity;

        // Iniciamos la rutina de parpadeo
        StartCoroutine(FlickerRoutine());
    }

    private System.Collections.IEnumerator FlickerRoutine()
    {
        while (true)
        {
            int intervalBetweenFlickers = Random.Range(min, max);

            // Espera el intervalo antes de parpadear
            yield return new WaitForSeconds(intervalBetweenFlickers);

            // Cambia a la intensidad de parpadeo
            lightSource.intensity = flickerIntensity;

            // Espera la duración del parpadeo
            yield return new WaitForSeconds(flickerDuration);

            // Vuelve a la intensidad inicial
            lightSource.intensity = initialIntensity;

            yield return new WaitForSeconds(flickerDuration);

            // Cambia a la intensidad de parpadeo
            lightSource.intensity = flickerIntensity;

            // Espera la duración del parpadeo
            yield return new WaitForSeconds(flickerDuration);

            // Vuelve a la intensidad inicial
            lightSource.intensity = initialIntensity;
        }
    }
}
