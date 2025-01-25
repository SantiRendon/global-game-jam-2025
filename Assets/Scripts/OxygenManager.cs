using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public Slider oxygenSlider; // El slider que muestra el nivel de oxígeno
    public float maxOxygen = 100f; // Nivel máximo de oxígeno
    public float oxygenDepletionRate = 5f; // Cuánto oxígeno se pierde por segundo

    [Header("Illumination Settings")]
    public float maxIllumination = 1f; // Iluminación máxima
    public float minIllumination = 0.2f; // Iluminación mínima
    public Light playerLight; // Luz del jugador

    [Header("Oxygen Restore Settings")]
    public GameObject nearestOxygenSource; // Objeto más cercano que restaura oxígeno
    public float detectionRadius = 10f; // Radio para buscar la fuente más cercana

    private float currentOxygen;
    private float currentIllumination;

    void Start()
    {
        // Inicializar oxígeno e iluminación
        currentOxygen = maxOxygen;
        currentIllumination = maxIllumination;
        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = currentOxygen;
    }

    void Update()
    {
        SimulateOxygenDepletion();
        UpdateIllumination();
        CheckOxygenLevel();
    }

    void SimulateOxygenDepletion()
    {
        // Reducir oxígeno con el tiempo
        currentOxygen -= oxygenDepletionRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
        oxygenSlider.value = currentOxygen;
    }

    void UpdateIllumination()
    {
        // Reducir la iluminación en función del oxígeno
        float illuminationFactor = Mathf.InverseLerp(0, maxOxygen, currentOxygen);
        currentIllumination = Mathf.Lerp(minIllumination, maxIllumination, illuminationFactor);

        if (playerLight != null)
        {
            playerLight.intensity = currentIllumination;
        }
    }

    void CheckOxygenLevel()
    {
        if (currentOxygen <= 0)
        {
            HighlightNearestOxygenSource();
        }
    }

    void HighlightNearestOxygenSource()
    {
        // Encontrar el objeto más cercano dentro del radio
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        float closestDistance = detectionRadius;
        GameObject closestSource = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("OxygenSource"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSource = collider.gameObject;
                }
            }
        }

        if (closestSource != null)
        {
            nearestOxygenSource = closestSource;
            // Aquí puedes añadir efectos visuales como un destello
            Debug.Log($"Destacando fuente de oxígeno: {nearestOxygenSource.name}");
            // Ejemplo de destello:
            StartCoroutine(OxygenSourceBlinkEffect(closestSource));
        }
    }

    IEnumerator OxygenSourceBlinkEffect(GameObject source)
    {
        Renderer renderer = source.GetComponent<Renderer>();
        if (renderer != null)
        {
            for (int i = 0; i < 5; i++)
            {
                renderer.enabled = !renderer.enabled;
                yield return new WaitForSeconds(0.2f);
            }
            renderer.enabled = true;
        }
    }
}
