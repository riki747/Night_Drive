using UnityEngine;

public class CarControllerNight : MonoBehaviour 
{
    // ==========================================
    // REFERENSI JOYSTICK MOBILE
    // ==========================================
    [Header("Mobile Input")]
    [SerializeField] private Joystick joystick; // Masukkan objek Fixed Joystick dari Canvas ke sini

    // ==========================================
    // PENGATURAN INPUT & STATUS MOBIL
    // ==========================================
    private float horizontalInput, verticalInput;
    private float currentSteerAngle;
    private bool isBraking;
    private bool isMobileBrakePressed; // Untuk tombol UI Rem
    
    // ==========================================
    // PENGATURAN TENAGA, REM, & BELOK
    // ==========================================
    [Header("Car Settings")]
    [SerializeField] private float motorForce = 1500f; 
    [SerializeField] private float brakeForce = 10000f; 
    [SerializeField] private float maxSteerAngle = 35f; 

    // ==========================================
    // SLOT WHEEL COLLIDER (Fisika Roda)
    // ==========================================
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    // ==========================================
    // SLOT WHEEL TRANSFORM (Visual 3D Roda)
    // ==========================================
    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void FixedUpdate()
    {
        GetInput();        
        HandleMotor();     
        HandleSteering();  
        UpdateWheels();    
    }

    private void GetInput()
    {
        // 1. Ambil input dari Joystick jika ada interaksi
        if (joystick != null && (joystick.Horizontal != 0 || joystick.Vertical != 0))
        {
            horizontalInput = joystick.Horizontal;
            verticalInput = joystick.Vertical;
        }
        else // Fallback ke Keyboard untuk testing di PC
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        
        // 2. Logika Rem (Bisa dari Keyboard ENTER atau Tombol UI Mobile)
        isBraking = Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter) || isMobileBrakePressed;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;

        float currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking(currentBrakeForce);
    }

    private void ApplyBraking(float force)
    {
        frontLeftWheelCollider.brakeTorque = force;
        frontRightWheelCollider.brakeTorque = force;
        rearLeftWheelCollider.brakeTorque = force;
        rearRightWheelCollider.brakeTorque = force;
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

    // ==========================================
    // FUNGSI UNTUK TOMBOL REM DI UI MOBILE
    // ==========================================
    public void HoldBrake()
    {
        isMobileBrakePressed = true;
    }

    public void ReleaseBrake()
    {
        isMobileBrakePressed = false;
    }
}