using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Joystick")]
    public FixedJoystick joystick;

    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    [Header("Settings")]
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float reverseForce = 0.5f;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [Header("UI & Camera System")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject freeLookCamera;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody carRigidbody;
    private bool isPaused = false;

    private bool isGasPressed = false;
    private bool isBrakePressed = false;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        carRigidbody = GetComponent<Rigidbody>();

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            isGasPressed = false;
            isBrakePressed = false;
        }
    }

    private void FixedUpdate()
    {
        if (isPaused) return;
        carRigidbody.AddForce(-transform.up * 10f * carRigidbody.linearVelocity.magnitude);

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        if (joystick != null)
        {
            horizontalInput = joystick.Horizontal;
            
            // ==========================================
            // OBAT BELOK SENDIRI (DEADZONE 10%)
            // ==========================================
            if (Mathf.Abs(horizontalInput) < 0.1f)
            {
                horizontalInput = 0f;
            }
        }

        float forwardSpeed = Vector3.Dot(carRigidbody.linearVelocity, transform.forward);

        if (isBrakePressed)
        {
            if (forwardSpeed > 0.1f)
            {
                verticalInput = 0f;
                isBreaking = true;
            }
            else
            {
                verticalInput = -reverseForce;
                isBreaking = false;
            }
        }
        else if (isGasPressed)
        {
            verticalInput = 1f;
            isBreaking = false;
        }
        else
        {
            // Pastikan motor mati total saat dilepas
            verticalInput = 0f;
            isBreaking = false;
        }
    }

   private void HandleMotor()
{
    if (isBreaking)
    {
        // 1. Saat ngerem: Matikan gas total, aktifkan rem penuh
        frontLeftWheelCollider.motorTorque = 0f;
        frontRightWheelCollider.motorTorque = 0f;
        currentbreakForce = breakForce; 
    }
    else if (!isGasPressed && !isBrakePressed)
    {
        // 2. Saat lepas tangan: Matikan gas total, aktifkan rem pasif (gaya gesek)
        frontLeftWheelCollider.motorTorque = 0f;
        frontRightWheelCollider.motorTorque = 0f;
        currentbreakForce = breakForce * 0.5f; 
        
        // Jurus diam seperti restart
        if (carRigidbody.linearVelocity.magnitude < 0.5f)
        {
            carRigidbody.linearVelocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;
            carRigidbody.Sleep();
        }
    }
    else
    {
        // 3. Saat jalan (Gas/Mundur): Jalankan motor, matikan rem
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = 0f; 
    }

    // Terapkan gaya rem ke roda depan & belakang
    ApplyBreaking();
}
    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    public void PointerDownGas() { isGasPressed = true; }
    public void PointerUpGas() { isGasPressed = false; }

    public void PointerDownBrake() { isBrakePressed = true; }
    public void PointerUpBrake() { isBrakePressed = false; }

    public void RestartPosition()
    {
        carRigidbody.isKinematic = true;
        transform.position = startPosition;
        transform.rotation = startRotation;
        horizontalInput = 0f;
        verticalInput = 0f;
        isGasPressed = false;
        isBrakePressed = false;
        
        currentbreakForce = breakForce;
        ApplyBreaking();

        carRigidbody.isKinematic = false;
        carRigidbody.linearVelocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
        carRigidbody.Sleep();

        isPaused = false;
        Time.timeScale = 1f;

        if (joystick != null) joystick.gameObject.SetActive(true);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        if (freeLookCamera != null)
        {
            freeLookCamera.SetActive(false);
            freeLookCamera.SetActive(true);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (joystick != null) joystick.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (joystick != null) joystick.gameObject.SetActive(true);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        }
    }
}