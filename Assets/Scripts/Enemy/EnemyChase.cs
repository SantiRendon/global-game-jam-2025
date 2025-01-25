using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float detectionRange = 10f; // Rango para comenzar a seguir al jugador
    public float stopFollowRange = 15f; // Rango para dejar de seguir al jugador
    public float moveSpeed = 3f; // Velocidad de movimiento del enemigo

    private bool isFollowing = false; // Indica si el enemigo está siguiendo al jugador
    private Vector2 originalPosition; // Posición original del enemigo

    void Start()
    {
        // Guardamos la posición inicial del enemigo
        originalPosition = transform.position;
    }

    void Update()
    {
        if (player == null)
            return; // Si no hay jugador asignado, no hacer nada

        // Calcular la distancia entre el enemigo y el jugador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Comenzar a seguir si el jugador está dentro del rango de detección
        if (distanceToPlayer <= detectionRange)
        {
            isFollowing = true;
        }
        // Dejar de seguir si el jugador está fuera del rango de seguimiento
        else if (distanceToPlayer > stopFollowRange)
        {
            isFollowing = false;
        }

        // Si está siguiendo, moverse hacia el jugador
        if (isFollowing)
        {
            FollowPlayer();
        }
        // Si no está siguiendo, regresar a la posición original
        else
        {
            ReturnToOriginalPosition();
        }
    }

    void FollowPlayer()
    {
        // Calcular la dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Mover al enemigo hacia el jugador
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void ReturnToOriginalPosition()
    {
        // Mover al enemigo de vuelta a su posición original
        transform.position = Vector2.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
    }
}
