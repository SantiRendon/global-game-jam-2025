using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuerca : MonoBehaviour
{
    [SerializeField] private GameObject bubble; // Referencia a la burbuja
    [SerializeField] private GameObject containedObject; // Referencia al objeto dentro de la burbuja
    [SerializeField] private Transform respawnAreaMin; // Límite inferior del área de reaparición
    [SerializeField] private Transform respawnAreaMax; // Límite superior del área de reaparición
    [SerializeField] private float respawnTime = 5f; // Tiempo antes de que reaparezca
    [SerializeField] private float fallSpeed = 1f; // Velocidad de caída del objeto

    private bool isPlayerNearby = false; // Si el jugador está cerca
    private bool isObjectCollected = false; // Si el jugador ya recogió el objeto
    private float timeSincePlayerLeft; // Tiempo desde que el jugador salió del rango
    private Transform player; // Referencia al jugador

    private void Start()
    {
        ResetObject(); // Configura el objeto a su estado inicial
    }

    private void Update()
    {
        if (!isPlayerNearby && !isObjectCollected)
        {
            timeSincePlayerLeft += Time.deltaTime;

            // Reaparece el objeto si el tiempo límite se cumple
            if (timeSincePlayerLeft >= respawnTime)
            {
                RespawnObject();
            }
        }
        else
        {
            timeSincePlayerLeft = 0f; // Reinicia el tiempo si el jugador está cerca
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isObjectCollected)
        {
            isPlayerNearby = true;
            player = collision.transform;

            // Ejecuta la animación de la burbuja
            Animator bubbleAnimator = bubble.GetComponent<Animator>();
            if (bubbleAnimator != null)
            {
                bubbleAnimator.SetTrigger("Pop");
            }

            // Hace que el objeto comience a caer
            containedObject.GetComponent<Rigidbody2D>().gravityScale = fallSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    public void PickUpObject()
    {
        if (isPlayerNearby && !isObjectCollected)
        {
            isObjectCollected = true;
            containedObject.transform.SetParent(player); // Hace que el objeto se mueva con el jugador
            containedObject.transform.localPosition = Vector3.up; // Posición relativa al jugador
            containedObject.GetComponent<Rigidbody2D>().isKinematic = true; // Detiene la física
        }
    }

    public void DropObject()
    {
        if (isObjectCollected)
        {
            isObjectCollected = false;
            containedObject.transform.SetParent(null); // Suelta el objeto
            containedObject.GetComponent<Rigidbody2D>().isKinematic = false; // Reactiva la física
        }
    }

    private void RespawnObject()
    {
        // Reposiciona la burbuja en una posición aleatoria dentro del área de reaparición
        float newX = Random.Range(respawnAreaMin.position.x, respawnAreaMax.position.x);
        float newY = Random.Range(respawnAreaMin.position.y, respawnAreaMax.position.y);
        bubble.transform.position = new Vector2(newX, newY);

        ResetObject();
    }

    private void ResetObject()
    {
        // Restaura el estado inicial de la burbuja y el objeto
        bubble.SetActive(true);
        containedObject.transform.SetParent(bubble.transform);
        containedObject.transform.localPosition = Vector3.zero;
        containedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        isObjectCollected = false;
        isPlayerNearby = false;
        timeSincePlayerLeft = 0f;
    }
}
