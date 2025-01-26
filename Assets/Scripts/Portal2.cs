using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal2 : MonoBehaviour
{
    public GameObject texto;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("portal"))
        {
            texto.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("portal"))
        {
            texto.SetActive(false);
        }
    }
}
