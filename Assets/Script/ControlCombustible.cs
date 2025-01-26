using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlCombustible : MonoBehaviour
{
    public Slider SliderCombustible;
    public GameObject FillRellenoCombustible;
    public float combustibleMaximo = 100f;
    public float consumoPorSegundo = 1f;
    private float combustibleActual;

    void Start()
    {
        combustibleActual = combustibleMaximo;
        SliderCombustible.maxValue = combustibleMaximo;
        SliderCombustible.value = combustibleActual;

        if(FillRellenoCombustible != null )
        {
            FillRellenoCombustible.SetActive(true);
        }
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && combustibleActual > 0)
        {
            consumirCombustible();
        }
    }
    public void consumirCombustible()
    {
        combustibleActual -= consumoPorSegundo * Time.deltaTime;
        combustibleActual = Mathf.Clamp(combustibleActual, 0, combustibleMaximo);
        SliderCombustible.value = combustibleActual;

        if(FillRellenoCombustible != null )
        {
            FillRellenoCombustible.SetActive(combustibleActual > 0);
        }
    }
}
