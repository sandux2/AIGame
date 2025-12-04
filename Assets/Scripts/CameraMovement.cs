using UnityEngine;

public class CameraFollowBehind : MonoBehaviour
{
    public Transform target;         // player
    public Vector3 offset = new Vector3(1, 3 - 12); //position from the player
    public float smoothSpeed = 7f;  // smooth follow

    void LateUpdate()
    {
        if (!target) return;

        // Position camera behind player relative to its rotation
        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Always look at player's upper body
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}