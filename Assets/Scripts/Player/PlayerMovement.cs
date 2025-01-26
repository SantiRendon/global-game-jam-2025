using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Velocidad de movimiento base
    public float moveSpeed = 3f;

    // Resistencia del agua (inercia)
    public float drag = 7f;

    private Vector2 velocity; // Velocidad actual del jugador
    private Rigidbody2D rb;

    // Variables para controlar la rotación
    private float targetYRotation = 0f; // Rotación objetivo en el eje Y
    public float rotationYSmoothness = 2f; // Suavidad de la transición en el eje Y
    public float rotationZSmoothness = 5f; // Suavidad de la transición en el eje Z

    // Referencia al GasolineManager
    public GasolineManager gasolineManager;

    // Velocidad máxima para evitar que se salga de la pantalla
    public float maxSpeed = 5f;

    // Referencia al Animator
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Sin gravedad en el agua
        rb.drag = drag; // Aplicamos resistencia

        // Obtener referencia al Animator
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No se encontró un componente Animator en el jugador.");
        }
    }

    void Update()
    {
        // Capturar la entrada del jugador (teclado)
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (gasolineManager != null)
        {
            // Calculamos la velocidad deseada utilizando la velocidad actual del GasolineManager
            Vector2 targetVelocity = new Vector2(moveX, moveY) * gasolineManager.CurrentSpeed;

            // Limitar la velocidad para evitar que se salga de la pantalla
            targetVelocity = Vector2.ClampMagnitude(targetVelocity, maxSpeed);

            // Interpolamos hacia la velocidad deseada para un movimiento más suave
            velocity = Vector2.Lerp(velocity, targetVelocity, Time.deltaTime * 10f);

            // Cambiar rotación Y según la dirección horizontal
            if (moveX < 0) // Mover a la izquierda
            {
                targetYRotation = 180f; // Girar hacia la izquierda
            }
            else if (moveX > 0) // Mover a la derecha
            {
                targetYRotation = 0f; // Girar hacia la derecha
            }

            // Suavizar la rotación del eje Y
            float currentYRotation = Mathf.LerpAngle(transform.eulerAngles.y, targetYRotation, Time.deltaTime * rotationYSmoothness);

            // Rotar el sprite del jugador hacia la dirección del movimiento
            if (velocity != Vector2.zero)
            {
                float angleZ = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg; // Rotación del eje Z

                // Suavizar la transición del eje Z
                float currentZRotation = Mathf.LerpAngle(transform.eulerAngles.z, angleZ, Time.deltaTime * rotationZSmoothness);

                // Aplicar ambas rotaciones (eje Y y Z)
                transform.rotation = Quaternion.Euler(new Vector3(0, currentYRotation, currentZRotation));
            }

            // Activar o desactivar la animación de correr
            bool isMoving = velocity.magnitude > 0.1f; // Detectar si el jugador se está moviendo
            animator.SetBool("correr", isMoving); // Cambiar el estado del parámetro "correr"
        }
        else
        {
            // Mostrar una advertencia si no se asigna el GasolineManager
            Debug.LogWarning("GasolineManager no asignado en PlayerMovement.");
        }
    }

    void FixedUpdate()
    {
        // Aplicamos la velocidad al Rigidbody
        rb.velocity = Vector2.ClampMagnitude(velocity, maxSpeed); // Limitar velocidad en el Rigidbody
    }
}
