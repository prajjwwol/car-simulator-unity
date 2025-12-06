using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class CarEngineSound : MonoBehaviour
{
    [Header("Pitch Settings")]
    public float idlePitch = 0.9f;     
    public float maxPitch = 2.0f;      

    [Header("Volume Settings")]
    public float idleVolume = 0.2f;   
    public float maxVolume = 0.8f;     

    [Header("Speed Settings")]
    public float maxSpeed = 40f;       

    private AudioSource engineAudio;
    private Rigidbody rb;

    void Awake()
    {
        engineAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        engineAudio.loop = true;

        if (!engineAudio.isPlaying)
            engineAudio.Play();

        engineAudio.pitch = idlePitch;
        engineAudio.volume = idleVolume;
    }

    void Update()
    {
        
        float accelInput = Mathf.Abs(Input.GetAxis("Vertical"));   

        
        float speed = rb.linearVelocity.magnitude;
        float speedT = Mathf.Clamp01(speed / maxSpeed);            

        
        float engineLoad = Mathf.Max(accelInput, speedT);

        
        float targetPitch = Mathf.Lerp(idlePitch, maxPitch, engineLoad);

        float targetVolume = Mathf.Lerp(idleVolume, maxVolume, engineLoad);

         
        engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, targetPitch, Time.deltaTime * 5f);
        engineAudio.volume = Mathf.Lerp(engineAudio.volume, targetVolume, Time.deltaTime * 5f);
    }
}