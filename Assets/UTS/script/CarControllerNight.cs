using UnityEngine;

public class CarControllerNight : MonoBehaviour 
{
    // ==========================================
    // PENGATURAN INPUT & STATUS MOBIL
    // ==========================================
    private float horizontalInput, verticalInput;
    private float currentSteerAngle;
    private bool isBraking;
    
    // ==========================================
    // PENGATURAN TENAGA, REM, & BELOK
    // ==========================================
    [SerializeField] private float motorForce = 1500f; // Tenaga mesin untuk maju/mundur
    [SerializeField] private float brakeForce = 10000f; // Tenaga cengkeraman rem (harus lebih besar dari motorForce)
    [SerializeField] private float maxSteerAngle = 35f; // Sudut belok (dikurangi dikit biar gak melintir)

    // ==========================================
    // SLOT WHEEL COLLIDER (Fisika Roda)
    // ==========================================
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // ==========================================
    // SLOT WHEEL TRANSFORM (Visual 3D Roda)
    // ==========================================
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void FixedUpdate()
    {
        GetInput();        
        HandleMotor();     
        HandleSteering();  
        UpdateWheels();    
    }

    private void GetInput()
    {
        // Kanan/Kiri (A/D)
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Maju/Mundur (W/S) - SEKARANG MANUAL, BISA MUNDUR!
        verticalInput = Input.GetAxis("Vertical");
        
        // Tekan ENTER untuk REM PAKEM
        isBraking = Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter);
    }

    private void HandleMotor()
    {
        // Menyalurkan tenaga ke 4 roda (AWD) untuk maju dan mundur
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;

        // Mengecek apakah tombol ENTER ditekan
        float currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking(currentBrakeForce);
    }

    private void ApplyBraking(float force)
    {
        // Menerapkan rem ke semua roda
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
}