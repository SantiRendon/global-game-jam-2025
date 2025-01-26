using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAndObjectHandler : MonoBehaviour
{
    public Animator bubbleAnimator; // Referencia al Animator de la burbuja
    public GameObject containedObject; // Objeto dentro de la burbuja
    public float respawnTime = 5f; // Tiempo antes de que el objeto regrese a su posición inicial

    private Vector3 initialPosition; // Posición inicial del objeto
    private Quaternion initialRotation; // Rotación inicial del objeto
    private Rigidbody2D objectRb;
    private bool playerInTrigger = false; // Si el jugador está en el trigger
    private Coroutine respawnCoroutine;

    private void Start()
    {
        // Guardar la posición y rotación inicial del objeto
        if (containedObject != null)
        {
            initialPosition = containedObject.transform.position;
            initialRotation = containedObject.transform.rotation;
            objectRb = containedObject.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && containedObject != null)
        {
            // Detener el respawn si el jugador está en el trigger
            playerInTrigger = true;

            // Activar la animación de la burbuja y hacer que el objeto caiga
            bubbleAnimator.SetTrigger("Pop");
            objectRb.isKinematic = false;
            objectRb.gravityScale = 0.5f;

            // Si hay una corrutina de respawn corriendo, detenerla
            if (respawnCoroutine != null)
                StopCoroutine(respawnCoroutine);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Cuando el jugador salga del trigger, permitir que el objeto desaparezca
            playerInTrigger = false;

            // Iniciar la corrutina de respawn
            respawnCoroutine = StartCoroutine(RespawnTimer());
        }
    }

    private IEnumerator RespawnTimer()
    {
        // Esperar el tiempo límite
        yield return new WaitForSeconds(respawnTime);

        if (!playerInTrigger) // Si el jugador no está cerca
        {
            // Regresar el objeto a su posición inicial
            containedObject.transform.position = initialPosition;
            containedObject.transform.rotation = initialRotation;
            containedObject.SetActive(true);

            // Restaurar valores iniciales
            objectRb.isKinematic = true;
            objectRb.gravityScale = 0;
        }
    }
}
