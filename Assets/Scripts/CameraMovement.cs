using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // the object to follow (player)
    public Vector3 offset;        // camera offset position
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (!target) return;

        // desired camera position
        Vector3 desiredPosition = target.position + offset;

        // smooth motion
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // look at the player
        transform.LookAt(target);
    }
}
