using UnityEngine;

public class OxygenPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el jugador tocó el objeto
        if (collision.CompareTag("Player"))
        {
            // Accede al script OxygenManager desde un GameObject que contiene el script
            OxygenManager oxygenManager = FindObjectOfType<OxygenManager>();

            // Verifica si el OxygenManager está presente en la escena
            if (oxygenManager != null)
            {
                oxygenManager.RestoreOxygen(); // Restaura el oxígeno al 100%
                Debug.Log("Oxígeno restaurado.");
            }
            else
            {
                Debug.LogWarning("No se encontró OxygenManager en la escena.");
            }

            // Destruir el objeto después de ser recogido
            Destroy(gameObject);
        }
    }
}
