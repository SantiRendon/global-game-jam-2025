using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabObject : MonoBehaviour
{
    [SerializeField] private Transform grabPoint; // Punto donde se sostiene el objeto
    [SerializeField] private float grabRadius = 1f; // Radio de detección de objetos cercanos
    [SerializeField] private LayerMask grabbableLayer; // Capa de los objetos que se pueden agarrar

    private GameObject grabbedObject; // Objeto actualmente agarrado

    private void Update()
    {
        // Detectar si se presiona la tecla de agarrar (espacio)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grabbedObject == null)
            {
                // Intentar agarrar un objeto
                TryGrabObject();
            }
            else
            {
                // Soltar el objeto actual
                ReleaseObject();
            }
        }
    }

    private void TryGrabObject()
    {
        // Detectar objetos dentro del radio de agarre
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, grabRadius, grabbableLayer);

        if (colliders.Length > 0)
        {
            grabbedObject = colliders[0].gameObject; // Agarrar el primer objeto encontrado
            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.isKinematic = true; // Desactivar la física para "sujetar" el objeto
                grabbedObject.transform.position = grabPoint.position; // Mover el objeto al punto de agarre
                grabbedObject.transform.SetParent(transform); // Hacer que siga al jugador
            }
        }
    }

    private void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.isKinematic = false; // Reactivar la física
                grabbedObject.transform.SetParent(null); // Desasociar del jugador
                grabbedObject = null; // Dejar de "agarrar" el objeto
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el radio de detección en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
}
