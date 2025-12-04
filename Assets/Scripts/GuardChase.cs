using UnityEngine;
using UnityEngine.AI;

public class GuardChase : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing = false;

    [Header("Chase Settings")]
    public float catchDistance = 1.5f;

    [Header("Door Reference")]
    public DoorController door; // reference to door

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // get NavMeshAgent component
    }

    void Update()
    {
        // chase logic
        if (isChasing && player != null)
        {
            agent.SetDestination(player.position);

            // distance check
            float distance = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(player.position.x, 0, player.position.z)
            );

            if (distance <= catchDistance)
            {
                Debug.Log("Player caught!");
                isChasing = false;
                agent.ResetPath();

                if (door != null && door.isOpen)
                    door.ToggleDoor(); // close after catching
            }
        }
    }

    // called by OwnerDetection
    public void StartChasing(Transform playerTransform) // assign player to chase
    {
        player = playerTransform;
        isChasing = true;
        Debug.Log("Guard started chasing the player!"); 

        if (door != null)
        {
            Debug.Log("Guard interacting with door: " + door.name);
            if (!door.isOpen)
                door.ToggleDoor();
        }
        else
        {
            Debug.LogWarning("Door reference not assigned in Inspector!");
        }
    }

    public void StopChasing() // stop chasing player
    {
        isChasing = false;
        agent.ResetPath();
        Debug.Log("Guard stopped chasing.");

        if (door != null && door.isOpen)
            door.ToggleDoor(); // close door when stopping
    }
}