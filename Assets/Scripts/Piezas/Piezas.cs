using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Piezas : MonoBehaviour
{
    public Text scoreText; // Asigna aquí el Text del Canvas
    private int score = 0; // Puntuación inicial

    private void Start()
    {
        UpdateScoreText(); // Inicializa el texto con la puntuación inicial
    }

    private void Update()
    {
        if(score > 5)
        {
            SceneManager.LoadScene(3);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sub"))
        {
            Destroy(collision.gameObject); // Destruye el objeto
            score++; // Incrementa la puntuación
            UpdateScoreText(); // Actualiza el texto en el Canvas
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score + "/5"; // Actualiza el texto
    }
}
