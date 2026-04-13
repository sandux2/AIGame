using UnityEngine;
using UnityEngine.AI;

public class OwnerPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;   // Points to move between
    public int currentPoint = 0;
    private NavMeshAgent agent;
    private Animator animator;
    bool isIdle = false;
    public float idleTime = 2f; // time to stay idle at each point
    public AudioSource footstepSound; // sound effect for footsteps when owner is walking
   

    void Start() 
    {
        agent = GetComponent<NavMeshAgent>(); // get NavMeshAgent component
        animator = GetComponentInChildren<Animator>(); // get Animator component
        if (patrolPoints.Length == 0) // check for patrol points
        {
            Debug.LogWarning("No patrol points assigned to owner!");
            return;
        }
        GoToNextPoint();
    }

    void GoToNextPoint() // set next destination
    {
        isIdle = false; // reset idle state
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
        animator.SetBool("IsWalking", true); // play walking animation
        footstepSound.Play(); // play footstep sound when walking
        /*if(currentPoint > patrolPoints.Length)
        {
            currentPoint = 0;
        } */
    }

    void Update() // check if reached destination
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f) // close to destination
        {
            if(isIdle != true) // if not already idle
            {
                isIdle = true; // set idle state
                animator.SetBool("IsWalking", false); // stop walking animation
                Invoke("GoToNextPoint", idleTime); // wait and go to next point
                footstepSound.Stop(); // stop footstep sound when idle
            }
        }
    } 
    public void StartChasingPlayer(Transform player) // start chasing player
{
    GetComponent<UnityEngine.AI.NavMeshAgent>().destination = player.position; // set destination to player
    Debug.Log("Guard is chasing the player!"); 
}

}
