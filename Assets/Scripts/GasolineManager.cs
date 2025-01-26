using UnityEngine;
using UnityEngine.UI;

public class GasolineManager : MonoBehaviour
{
    [Header("Gasoline Settings")]
    public Slider gasolineSlider; // El slider que muestra el nivel de gasolina
    public float maxGasoline = 100f; // Cantidad máxima de gasolina
    public float sprintDepletionRate = 20f; // Tasa de consumo de gasolina al hacer sprint (por segundo)

    [Header("Player Movement Settings")]
    public Rigidbody2D playerRb; // Referencia al Rigidbody del jugador
    public float normalSpeed = 5f; // Velocidad normal del jugador
    public float sprintSpeed = 8f; // Velocidad al hacer sprint

    private float currentGasoline;
    public bool IsSprinting { get; private set; } // Indica si el jugador está haciendo sprint

    private Vector2 inputDirection; // Dirección de movimiento

    // Propiedad para obtener la velocidad actual
    public float CurrentSpeed
    {
        get
        {
            // Si está haciendo sprint y hay gasolina, se mueve más rápido
            if (IsSprinting && currentGasoline > 0)
            {
                return sprintSpeed;
            }
            // Si no está haciendo sprint, o no hay gasolina, velocidad normal
            return normalSpeed;
        }
    }

    public void FillGasoline()
    {
        currentGasoline = maxGasoline; // Llena la gasolina al máximo
        gasolineSlider.value = currentGasoline; // Actualiza el slider
    }

    void Start()
    {
        // Inicializar el nivel de gasolina
        currentGasoline = maxGasoline;
        gasolineSlider.maxValue = maxGasoline;
        gasolineSlider.value = currentGasoline;
    }

    void Update()
    {
        HandleInput();
        UpdateGasolineLevel();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void HandleInput()
    {
        // Detectar la dirección de movimiento
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        inputDirection = new Vector2(moveX, moveY).normalized; // Normalizamos para mantener velocidad constante

        // Detectar si el jugador está presionando la tecla de sprint
        IsSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Evitar que haga sprint si no hay gasolina
        if (currentGasoline <= 0)
        {
            IsSprinting = false;
        }
    }

    void UpdateGasolineLevel()
    {
        if (IsSprinting)
        {
            // Reducir gasolina mientras el jugador hace sprint
            currentGasoline -= sprintDepletionRate * Time.deltaTime;
            currentGasoline = Mathf.Clamp(currentGasoline, 0, maxGasoline);
        }

        // Actualizar el slider de gasolina
        gasolineSlider.value = currentGasoline;
    }

    void MovePlayer()
    {
        // Aplicar el movimiento al Rigidbody del jugador
        playerRb.velocity = inputDirection * CurrentSpeed;
    }

   

}
