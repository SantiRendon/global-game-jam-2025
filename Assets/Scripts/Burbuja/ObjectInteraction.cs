using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public float gravedadCuandoActivo = 0.004f; // Gravedad personalizada cuando el objeto está suelto
    public float tiempoReaparicion = 5f; // Tiempo antes de reaparecer
    public Vector2 zonaMin; // Coordenadas mínimas de la zona aleatoria
    public Vector2 zonaMax; // Coordenadas máximas de la zona aleatoria

    private bool jugadorEnTrigger = false; // Si el jugador está en el trigger
    private bool siendoLlevado = false; // Si el objeto está siendo llevado
    private bool contadorActivo = false; // Si el contador está activo

    private Rigidbody2D rb; // Referencia al Rigidbody del objeto
    private Vector3 posicionInicial; // Posición inicial del objeto
    private Quaternion rotacionInicial; // Rotación inicial del objeto
    private float tiempoFuera; // Contador de tiempo fuera

    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // El objeto comienza como cinemático
        posicionInicial = transform.position; // Guardar posición inicial
        rotacionInicial = transform.rotation; // Guardar rotación inicial
    }

    private void Update()
    {
        if (jugadorEnTrigger && !siendoLlevado && Input.GetKeyDown(KeyCode.E))
        {
            RecogerObjeto(); // Recoger el objeto si el jugador está en el trigger
        }
        else if (siendoLlevado && Input.GetKeyDown(KeyCode.E))
        {
            SoltarObjeto(); // Soltar el objeto si está siendo llevado
        }

        if (contadorActivo)
        {
            tiempoFuera += Time.deltaTime;
            if (tiempoFuera >= tiempoReaparicion)
            {
                ReaparecerObjeto(); // Reaparecer cuando el tiempo se agote
            }
        }
    }

    private void RecogerObjeto()
    {
        siendoLlevado = true;
        rb.isKinematic = true; // Mantener cinemático mientras es llevado
        rb.velocity = Vector2.zero; // Detener cualquier movimiento residual
        transform.SetParent(PlayerController.instancia.carryPoint); // Hacer hijo del punto de carga del jugador
        transform.localPosition = Vector3.zero; // Alinear con el punto de carga
        PlayerController.instancia.LlevarObjeto(this); // Notificar al jugador que lleva un objeto
        contadorActivo = false; // Detener el contador si estaba activo
        tiempoFuera = 0f; // Reiniciar el contador
    }

    private void SoltarObjeto()
    {
        siendoLlevado = false;
        rb.isKinematic = false; // Volver a no cinemático
        transform.SetParent(null); // Liberar el objeto del jugador
        rb.gravityScale = gravedadCuandoActivo; // Aplicar la gravedad personalizada
        PlayerController.instancia.SoltarObjeto(); // Notificar al jugador que ya no lleva el objeto
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !siendoLlevado)
        {
            jugadorEnTrigger = true;
            rb.isKinematic = false; // Cambiar a no cinemático si no está siendo llevado
            rb.gravityScale = gravedadCuandoActivo; // Aplicar la gravedad personalizada
            contadorActivo = false; // Detener el contador si el jugador regresa
            tiempoFuera = 0f; // Reiniciar el contador
            animator.Play("pop");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !siendoLlevado)
        {
            jugadorEnTrigger = false;
            contadorActivo = true; // Activar el contador cuando el jugador se aleje
        }
    }

    private void ReaparecerObjeto()
    {
        contadorActivo = false; // Detener el contador
        tiempoFuera = 0f; // Reiniciar el contador
        rb.isKinematic = true; // Volver a cinemático
        rb.velocity = Vector2.zero; // Detener cualquier movimiento residual
        rb.gravityScale = 0f; // Desactivar la gravedad

        // Generar posición aleatoria dentro de la zona
        float x = Random.Range(zonaMin.x, zonaMax.x);
        float y = Random.Range(zonaMin.y, zonaMax.y);
        transform.position = new Vector3(x, y, posicionInicial.z);

        transform.rotation = rotacionInicial; // Restaurar la rotación inicial
        animator.Play("idle");
    }
}
