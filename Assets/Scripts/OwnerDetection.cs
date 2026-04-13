using System.Collections;
using FOV;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class OwnerDetection : MonoBehaviour
{
public float detectionRange = 5f; // range to detect player
    public GuardChase guard; // reference to GuardChase script
    public int currentFloor = 0; // 0 for downstairs, 1 for upstairs
    public NavMeshAgent agent;
     float speed = 2f; // owner movement speed
     public Transform player; // reference to player transform
        public float DetectDistance = 5f; // distance at which owner can detect player
    public float DetectDistanceCrouched = 3f; // reduced detection distance when player is crouched
    bool playerSpotted = false; // flag to prevent multiple detections
    public AudioSource footStepsSound; // sound effect for footsteps when owner is walking
    FieldOfView view;
    public bool isBehaviorTreeOwner = false; // flag to indicate if owner is controlled by behavior tree
    public BehaviorGraphAgent behaviorGraphAgent; // reference to BehaviorGraphAgent component for behavior tree control
    void Start()
    {
        view = GetComponent<FieldOfView>(); // get FieldOfView component
        agent = GetComponent<NavMeshAgent>(); // get NavMeshAgent component
        speed = agent.speed; // set owner movement speed
        player = GameObject.FindGameObjectWithTag("Player").transform; // find player transform by tag
    }
   
    void Update()
    {
        if (view.Field<Transform>().ToArray().Length != 0)
        {
            Debug.Log("Owner spotted the player in field of view!");
            ownerDetectedPlayer(); // handle player detection
            playerSpotted = true; // set flag to prevent multiple detections
        } // check if player is in field of view
        
        if(player.GetComponent<PlayerMovement>().IsCrouched)
        {
            view.radius = 4f; // reduce view radius when player is crouched
        }
        else
        {
            view.radius = 7f; // reset view radius when player is not crouched
        }
        //Debug.DrawRay(rayposition, transform.TransformDirection (new Vector3(1,0,1)) * detectionRange, Color.red); // visualize ray in editor
       // Debug.DrawRay(rayposition, transform.TransformDirection (new Vector3(-1,0,1)) * detectionRange, Color.red); // visualize ray
        
        /*if (Physics.Raycast(rayposition, transform.TransformDirection (new Vector3(1,0,1)), out RaycastHit hit, detectionRange)
        || Physics.Raycast(rayposition, transform.TransformDirection (new Vector3(-1,0,1)), out hit, detectionRange)) // raycast forward
        {
            if (hit.collider.CompareTag("Player") && !playerSpotted) // check for player tag
            {
                ownerDetectedPlayer(); // handle player detection
                playerSpotted = true; // set flag to prevent multiple detections

            }
        } */

        float DetectDistanceToUse = player.GetComponent<PlayerMovement>().IsCrouched ? DetectDistanceCrouched : DetectDistance; // use reduced distance if player is crouched

       /* if(Vector3.Distance(transform.position, player.position) < DetectDistanceToUse && !playerSpotted) // if player is far away, stop chasing
        {
           ownerDetectedPlayer(); // handle player detection
            playerSpotted = true; // set flag to prevent multiple detections
        } */
    }
    /*private void OnTriggerEnter(Collider other) // detect player entering owner's area
    {
        if (other.CompareTag("Player")) // check for player tag
        {
            ownerDetectedPlayer(); // handle player detection
        }
    } */

    IEnumerator ResetOwner() // reset owner after alerting guard
    {
        yield return new WaitForSeconds(20f); // wait for 20 seconds
        agent.isStopped = false; // resume owner movement
        agent.speed = speed; // reset owner speed
        GetComponentInChildren<Animator>().ResetTrigger("Terrified"); // reset animation trigger
        GetComponentInChildren<Animator>().SetTrigger("Reset"); // resume walking animation
        playerSpotted = false; // reset detection flag
    }

    void ownerDetectedPlayer()
    {
        Debug.Log("Owner spotted the player!");
        GetComponent<NavMeshAgent>().speed = 0; // stop owner movement
        GetComponent <NavMeshAgent>().isStopped = true; // stop owner movement
        GetComponentInChildren<Animator>().SetTrigger("Terrified"); // play alert animation
        StartCoroutine(ResetOwner()); // reset owner after alerting guard
        footStepsSound.Stop(); // stop footstep sound when owner is alerting
        if (guard != null)
        {
            if(isBehaviorTreeOwner)
            {
                behaviorGraphAgent.BlackboardReference.SetVariableValue("Player Spotted", true); // enable behavior tree control
                //behaviorGraphAgent.BlackboardReference.SetVariableValue("player spotted position", player.transform.position); // set player reference in blackboard
                //behaviorGraphAgent.BlackboardReference.SetVariableValue("IsDownstairs", currentFloor >= 1 ? false : true); // set floor reference in blackboard
                return;
            }
            
            {
                
            
          //  guard.StartChasing(other.transform); // tell guard to chase player
            guard.isChasing = true; // set chasing flag
            //guard.currentState = GuardChase.GuardState.arrive;
            guard.floor = currentFloor; // tell guard which floor player is on
            guard.lastKnownPlayerPosition = player.transform.position; // store last known player position
            }
        }
        else
        {
            if(isBehaviorTreeOwner)
            {
                behaviorGraphAgent.BlackboardReference.SetVariableValue("Player Spotted", true); // enable behavior tree control
                behaviorGraphAgent.BlackboardReference.SetVariableValue("player spotted position", player.transform.position); // set player reference in blackboard
                behaviorGraphAgent.BlackboardReference.SetVariableValue("IsDownstairs", currentFloor >= 1 ? false : true); // set floor reference in blackboard
                return;
            }
        }
    }
}
