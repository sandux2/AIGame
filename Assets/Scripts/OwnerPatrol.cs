using UnityEngine;
using UnityEngine.AI;

public class OwnerPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;   // Points to move between
    private int currentPoint = 0;
    private NavMeshAgent agent;

    void Start() 
    {
        agent = GetComponent<NavMeshAgent>(); // get NavMeshAgent component
        if (patrolPoints.Length == 0) // check for patrol points
        {
            Debug.LogWarning("No patrol points assigned to owner!");
            return;
        }
        GoToNextPoint();
    }

    void GoToNextPoint() // set next destination
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    void Update() // check if reached destination
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f) // close to destination
            GoToNextPoint(); // go to next point
    }
    public void StartChasingPlayer(Transform player) // start chasing player
{
    GetComponent<UnityEngine.AI.NavMeshAgent>().destination = player.position; // set destination to player
    Debug.Log("Guard is chasing the player!"); 
}

}
