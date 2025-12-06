using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Main Settings")]
    public float maxSpeed = 50f;           // Top speed forward (km/h feel)
    public float reverseSpeed = 20f;       // Top speed reverse
    public float maxTorque = 650f;         // Engine power
    public float turnSpeed = 3.5f;         // Steering sensitivity

    [Header("Physics Feel")]
    public float carMass = 1350f;
    public float drag = 0.08f;             // Air resistance
    public float rollingResistance = 12f;
    public float downforce = 2.8f;         // More grip at high speed
    public float lateralGrip = 1.35f;      // Sideways grip (tire grip)
    public float gripSpeedFade = 0.97f;    // Slight grip loss at very high speed

    [Header("Visual Feedback")]
    public float pitchAmount = 3f;
    public float rollAmount = 4f;
    public float pitchSpeed = 4f;
    public float rollSpeed = 6f;

    [Header("Runtime")]
    public int displaySpeed;               // 0-100 for your Arduino gauge

    private Rigidbody rb;
    private Vector3 velocity;
    private float currentThrottle = 0f;
    private float currentSteer = 0f;

    private float targetPitch = 0f;
    private float targetRoll = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Critical: Force correct Rigidbody settings from code (overrides Inspector mistakes)
        rb.mass = carMass;
        rb.linearDamping = 0f;                                   // We simulate drag ourselves
        rb.angularDamping = 8f;                            // Stops spinning like crazy
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        
        // Freeze X and Z rotation (prevents flipping), leave Y free for steering
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Input (works perfectly with steering wheels + pedals)
        currentSteer = Input.GetAxis("Horizontal");      // Left/Right
        float accelInput = Input.GetAxis("Vertical");    // Forward = 1, Brake/Reverse = -1

        // Smooth throttle response
        float targetThrottle = accelInput > 0.01f ? accelInput : accelInput * 0.6f; // Reverse is weaker
        currentThrottle = Mathf.Lerp(currentThrottle, targetThrottle, Time.deltaTime * 8f);

        // Current velocity in local space
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        float forwardSpeed = localVelocity.z;

        // ======================== ENGINE TORQUE ========================
        float torque = currentThrottle * maxTorque;
        float speedFactor = Mathf.Clamp01(1f - (Mathf.Abs(forwardSpeed) / maxSpeed));
        torque *= speedFactor * speedFactor; // Realistic power drop at high speed

        // ======================== DRAG & RESISTANCE ========================
        float speed = rb.linearVelocity.magnitude;
        float airDrag = speed * speed * drag;
        float rollRes = rollingResistance * speed;
        Vector3 resistingForce = -rb.linearVelocity.normalized * (airDrag + rollRes);

        // Downforce = more grip when fast
        float effectiveDownforce = downforce * speed * speed;

        // ======================== LATERAL GRIP (simple tire model) ========================
        float lateralVel = localVelocity.x;
        float forwardGripFactor = Mathf.Clamp01(1f - Mathf.Abs(lateralVel) / 20f);
        float currentGrip = lateralGrip * (1f + effectiveDownforce) * forwardGripFactor;
        currentGrip *= Mathf.Lerp(gripSpeedFade, 1f, speedFactor);

        Vector3 lateralForce = -transform.right * (lateralVel * currentGrip * 60f);

        // ======================== FINAL FORCES ========================
        Vector3 engineForce = transform.forward * torque;
        Vector3 totalForce = engineForce + resistingForce + lateralForce;

        // Apply force properly through Rigidbody
        rb.AddForce(totalForce * Time.deltaTime, ForceMode.Force);

        // Hard speed limiter
        if (rb.linearVelocity.magnitude > (currentThrottle >= 0 ? maxSpeed : reverseSpeed))
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (currentThrottle >= 0 ? maxSpeed : reverseSpeed);
        }

        // ======================== STEERING ========================
        float turnRate = turnSpeed * Mathf.Lerp(2.5f, 0.8f, Mathf.Clamp01(forwardSpeed / 15f));
        rb.AddTorque(Vector3.up * currentSteer * turnRate * 100f * Time.deltaTime, ForceMode.Force);

        // ======================== BODY TILT (Pitch & Roll) ========================
        targetPitch = -currentThrottle * pitchAmount;
        targetRoll = -currentSteer * rollAmount * Mathf.Clamp01(forwardSpeed / 10f);

        float smoothPitch = Mathf.LerpAngle(transform.localEulerAngles.x > 180 ? transform.localEulerAngles.x - 360 : transform.localEulerAngles.x, targetPitch, Time.deltaTime * pitchSpeed);
        float smoothRoll = Mathf.LerpAngle(transform.localEulerAngles.z > 180 ? transform.localEulerAngles.z - 360 : transform.localEulerAngles.z, targetRoll, Time.deltaTime * rollSpeed);

        transform.localRotation = Quaternion.Euler(smoothPitch, transform.localEulerAngles.y, smoothRoll);

        // ======================== SEND SPEED TO ARDUINO (0-100) ========================
        displaySpeed = Mathf.RoundToInt(Mathf.Abs(forwardSpeed) / maxSpeed * 100);
        displaySpeed = Mathf.Clamp(displaySpeed, 0, 100);

        if (SerialCommunicator.Instance != null)
            SerialCommunicator.Instance.SendSpeed(displaySpeed);
    }

    // Optional: see forces in Scene view
    void OnDrawGizmosSelected()
    {
        if (rb != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, rb.linearVelocity);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * 10);
        }
    }
}