using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float moveSpeed = 3f; // Velocidad de movimiento del enemigo
    public Transform[] waypoints; // Puntos de patrulla
    public float stoppingDistance = 0.1f; // Distancia mínima para considerar que el enemigo ha llegado a un punto de patrulla

    private int currentWaypointIndex = 0; // Índice del punto de patrulla actual

    void Update()
    {
        // Si no hay puntos de patrulla, no hacer nada
        if (waypoints.Length == 0)
            return;

        // Calcular la dirección hacia el punto de patrulla actual
        Vector2 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;

        // Mover al enemigo hacia el punto de patrulla actual
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);

        // Si el enemigo ha llegado al punto de patrulla actual, avanzar al siguiente
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) <= stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
