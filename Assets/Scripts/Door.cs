using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public float openAngle = 90f;
    public float speed = 2f;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private NavMeshObstacle obstacle;

    void Start()
    {
        closedRotation = transform.rotation; // initial rotation
        openRotation = Quaternion.Euler(0, transform.eulerAngles.y + openAngle, 0); // calculate open rotation
        obstacle = GetComponent<NavMeshObstacle>(); // get NavMeshObstacle component
    }

    void Update() // smooth door movement
    {
        if (isOpen)
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * speed);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, Time.deltaTime * speed);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        // Disable NavMesh obstacle when open so AI can pass
        if (obstacle != null)
            obstacle.carving = !isOpen;
    }
}