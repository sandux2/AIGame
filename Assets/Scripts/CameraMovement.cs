using UnityEngine;

public class CameraFollowBehind : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (!target) return;

        // World space offset (stabil)
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Look at player
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
