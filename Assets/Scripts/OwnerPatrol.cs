using UnityEngine;
using UnityEngine.AI;

public class OwnerPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;   // Points to move between
    private int currentPoint = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("No patrol points assigned to owner!");
            return;
        }
        GoToNextPoint();
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GoToNextPoint();
    }
    public void StartChasingPlayer(Transform player)
{
    GetComponent<UnityEngine.AI.NavMeshAgent>().destination = player.position;
    Debug.Log("Guard is chasing the player!");
}

}
