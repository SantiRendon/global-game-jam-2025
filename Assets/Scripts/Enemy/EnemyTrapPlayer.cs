using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrapPlayer : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float moveSpeed = 5f; // Velocidad del enemigo
    public float spawnIntervalMin = 3f; // Intervalo mínimo de aparición
    public float spawnIntervalMax = 8f; // Intervalo máximo de aparición
    public float spawnRangeX = 5f; // Rango en el eje X cercano al jugador
    public float depthY = -10f; // Profundidad donde el enemigo lleva al jugador
    public float timeToDisappear = 6f; // Tiempo máximo para desaparecer si no toca al jugador
    public float playerSpeedReductionRate = 0.5f; // Velocidad de reducción del jugador por segundo

    private Rigidbody2D playerRb; // Referencia al Rigidbody2D del jugador
    private Vector3 initialPosition; // Posición inicial del enemigo
    private bool isDraggingPlayer = false; // Indica si el enemigo está arrastrando al jugador
    private bool isActive = false; // Indica si el enemigo está activo

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        StartCoroutine(SpawnEnemy());
    }

    void Update()
    {
        if (!isActive) return;

        if (isDraggingPlayer)
        {
            // Arrastrar al jugador hacia las profundidades
            player.position = Vector2.MoveTowards(player.position, new Vector3(player.position.x, depthY, 0), moveSpeed * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(player.position.x, depthY, 0), moveSpeed * Time.deltaTime);
        }
        else
        {
            // Perseguir al jugador
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isDraggingPlayer = true;
            StartCoroutine(DragPlayerToDepth());
        }
    }

    IEnumerator DragPlayerToDepth()
    {
        // Reducir la velocidad del jugador gradualmente
        while (playerRb.velocity != Vector2.zero)
        {
            playerRb.velocity = Vector2.MoveTowards(playerRb.velocity, Vector2.zero, playerSpeedReductionRate * Time.deltaTime);
            yield return null;
        }

        // Esperar un breve momento mientras se arrastra
        yield return new WaitForSeconds(2f);

        // Desactivar el enemigo y regresar a la posición inicial
        isActive = false;
        transform.position = initialPosition;
        isDraggingPlayer = false;
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio antes de aparecer
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            // Elegir una posición cercana al jugador en el eje X
            float randomX = Random.Range(player.position.x - spawnRangeX, player.position.x + spawnRangeX);
            transform.position = new Vector3(randomX, initialPosition.y, 0);
            isActive = true;

            // Mover hacia el jugador por un tiempo definido
            yield return new WaitForSeconds(timeToDisappear);

            if (!isDraggingPlayer)
            {
                // Si no colisiona con el jugador, regresar y desactivarse
                isActive = false;
                transform.position = initialPosition;
            }
        }
    }

    IEnumerator Respawn()
    {
        // Desactivar el enemigo y regresar a la posición inicial
        isActive = false;
        transform.position = initialPosition;
        yield return null;
    }
}

