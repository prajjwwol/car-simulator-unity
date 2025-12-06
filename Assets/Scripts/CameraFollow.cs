using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -7);
    public float smoothSpeed = 5f;
    public bool lookAtTarget = true;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate desired position in local space (rotates with car)
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // Smoothly move camera to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Optionally look at the target
        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }
}
