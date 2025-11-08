using UnityEngine;
using UnityEngine.AI;

public class GuardChase : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private bool isChasing = false;

    [Header("Chase Settings")]
    public float catchDistance = 1.5f; // how close before stopping

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Follow the player while chasing
        if (isChasing && player != null)
        {
            agent.SetDestination(player.position);

            // Check distance only on the XZ plane (ignore Y height)
            Vector3 guardPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 playerPos = new Vector3(player.position.x, 0, player.position.z);

            float distance = Vector3.Distance(guardPos, playerPos);

            if (distance <= catchDistance)
            {
                Debug.Log("🚨 Player caught!");
                isChasing = false;
                agent.ResetPath(); // stop moving
            }
        }
    }

    // Called when the owner detects the player
    public void StartChasing(Transform playerTransform)
    {
        player = playerTransform;
        isChasing = true;
        Debug.Log("🚨 Guard started chasing the player!");
    }

    // Optional: stop chasing if player escapes
    public void StopChasing()
    {
        isChasing = false;
        agent.ResetPath();
    }
}
