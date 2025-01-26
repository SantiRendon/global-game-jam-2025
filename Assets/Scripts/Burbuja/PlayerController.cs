using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instancia; // Singleton para acceso global
    public Transform carryPoint; // Punto donde se lleva el objeto

    private ObjectInteraction objetoLlevado; // Objeto que el jugador está llevando actualmente

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LlevarObjeto(ObjectInteraction objeto)
    {
        if (objetoLlevado == null)
        {
            objetoLlevado = objeto; // Asignar el objeto como llevado
        }
    }

    public void SoltarObjeto()
    {
        objetoLlevado = null; // Liberar el objeto llevado
    }

    public bool PuedeLlevarObjeto()
    {
        return objetoLlevado == null; // Devuelve true si no lleva ningún objeto
    }
}
