using UnityEngine;
using UnityEngine.UI;

public class GasolineManager : MonoBehaviour
{
    [Header("Gasoline Settings")]
    public Slider gasolineSlider; // El slider que muestra el nivel de gasolina
    public float maxGasoline = 100f; // Cantidad máxima de gasolina
    public float sprintDepletionRate = 20f; // Tasa de consumo de gasolina al hacer sprint (por segundo)

    private float currentGasoline;
    private bool isSprinting;

    void Start()
    {
        // Inicializar el nivel de gasolina
        currentGasoline = maxGasoline;
        gasolineSlider.maxValue = maxGasoline;
        gasolineSlider.value = currentGasoline;
    }

    void Update()
    {
        HandleSprintInput();
        UpdateGasolineLevel();
    }

    void HandleSprintInput()
    {
        // Detectar si el jugador está presionando la tecla de sprint (Shift por defecto)
        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    void UpdateGasolineLevel()
    {
        if (isSprinting)
        {
            // Reducir gasolina mientras el jugador está haciendo sprint
            currentGasoline -= sprintDepletionRate * Time.deltaTime;
            currentGasoline = Mathf.Clamp(currentGasoline, 0, maxGasoline);
        }

        // Actualizar el valor del slider
        gasolineSlider.value = currentGasoline;
    }
}
