using FOV;
using UnityEngine;
using UnityEngine.AI;

public class GuardChase : MonoBehaviour
{
    private NavMeshAgent agent;

    private Transform player;

    public bool isChasing = false;
    public float DetectionRange = 5f;
    private bool leaveAfterSearch = false; // flag to determine if guard should return after searching
    private Animator animator;
    public Vector3 lastKnownPlayerPosition; // store last known player position
    private Vector3 initialPosition; // store guard's initial position for returning after search
    [Header("Search Settings")]
        
    public GameObject searchPointParentdownstairs; // parent object for downstairs search points
    public GameObject searchPointParentupstairs; // parent object for upstairs search points

    public Transform[] searchPointsdownstairs; // points to search when losing player
    public Transform[] searchPointsupstairs;
    public int floor = 0; // 0 for downstairs, 1 for upstairs
    public int currentSearchPoint = 1; // start from 1 to skip parent transform
    [Header("Chase Settings")]
    public float catchDistance = 1.5f;
    public float timer = 5f, resetTimer;// time to spend searching before giving up

    [Header("Door Reference")]
    public DoorController door;
    public float DetectDistance = 5f; // distance at which guard can detect player
    public float DetectDistanceCrouched = 3f; // reduced detection distance when player is crouched
    public GameObject LoseScreen; // UI screen for losing the game
    public FieldOfView view; // reference to FieldOfView component for owner detection
   

    public enum GuardState
    {
        Idle,
        arrive,
        Searching,
        lookingAround,
        leave,
        Chasing,
        Caught
    }
    public GuardState currentState = GuardState.Idle;      

    void Start()
    {
        resetTimer = timer;
        initialPosition = transform.position; // store initial position
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>(); // IMPORTANT
        player = GameObject.FindGameObjectWithTag("Player").transform;
        searchPointsdownstairs = searchPointParentdownstairs.GetComponentsInChildren<Transform>();
        searchPointsupstairs = searchPointParentupstairs.GetComponentsInChildren<Transform>();
        view = GetComponent<FieldOfView>();
        if (animator == null)
        {
            Debug.LogError("No Animator found on Guard or its children!");
        }
    }

    void Update()

    {

        switch (currentState)
        {
            case GuardState.Idle:
                IdleState();
                break;
            case GuardState.arrive:
                ArriveState();
                break;
            case GuardState.Searching:
                SearchState();
                break;
            
            case GuardState.lookingAround:
                LookingAroundState();
                break;
            case GuardState.leave:
                    ReturnToInitialPosition();
                break;
            case GuardState.Chasing:
                ChaseState();
                break;
                

            case GuardState.Caught:
                // Do nothing or add caught behavior here
                break;
        }
        /* if (isChasing && player != null)
        {
            agent.SetDestination(player.position);

            float speed = agent.velocity.magnitude;

            // Animation logic
            if (speed > 0.1f)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }

            float distance = Vector3.Distance(
                new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(player.position.x, 0, player.position.z)
            );

            if (distance <= catchDistance)
            {
                Debug.Log("Player caught!");

                isChasing = false;
                agent.ResetPath();

                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetTrigger("caught");

                if (door != null && door.isOpen)
                    door.ToggleDoor();
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        } */
    }

    public void IdleState()
    {
        leaveAfterSearch = false; // reset flag when idle
        if (isChasing)
                {
                    currentState = GuardState.arrive;
                    animator.SetBool("Run", true);
                    if (door != null && !door.isOpen)
                        door.ToggleDoor();
                    Debug.Log("Guard started chasing!");
                    

                }
    }
    public void ArriveState()
    {  
        Detection();
        if (lastKnownPlayerPosition != Vector3.zero)
        {
            agent.SetDestination(lastKnownPlayerPosition);
         
        if(Vector3.Distance(transform.position, lastKnownPlayerPosition) < 1f)
            {
                currentState = GuardState.lookingAround;
                animator.SetBool("Run", false);
                animator.SetBool("LookAround", true);
                Debug.Log("Guard started looking around!");
            }
        }
        
        
    }
    
    public void LookingAroundState()
    {
        Detection();
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if(leaveAfterSearch)
            {
                currentState = GuardState.leave;
                animator.SetBool("LookAround", false);
                animator.SetBool("Walk", true);
                Debug.Log("Guard is leaving after searching!");
                return;
            }
            currentState = GuardState.Searching;
            animator.SetBool("LookAround", false);
            animator.SetBool("Walk", true);
            Debug.Log("Guard is searching for the player!");
            timer = resetTimer; // reset timer for next time
        }
    }
    
    public void ChaseState()
    {
        float speed = agent.velocity.magnitude;
                        agent.SetDestination(player.position);


                

                float distance = Vector3.Distance(
                    new Vector3(transform.position.x, 0, transform.position.z),
                    new Vector3(player.position.x, 0, player.position.z)
                );

                if (distance <= catchDistance)
                {
                    Debug.Log("Player caught!");

                    currentState = GuardState.Caught;
                    agent.ResetPath();

                    animator.SetTrigger("Chaught");

                    player.gameObject.GetComponent<PlayerMovement>().enabled = false; // disable player movement
                    player.gameObject.GetComponent<PlayerInteract>().enabled = false; // disable player interaction

                    Invoke("CaughtPlayer", 1f); // delay to show caught animation
                    
                    

                    
                }
    }
    public void SearchState()
    {
        Transform[] searchPoints = floor == 0 ? searchPointsdownstairs : searchPointsupstairs; // choose search points based on floor
        
        Detection();
        

        if (searchPoints.Length == 0) return;

        agent.SetDestination(searchPoints[currentSearchPoint].position);

        if (Vector3.Distance(transform.position, searchPoints[currentSearchPoint].position) < 1f)
        {
            currentState = GuardState.lookingAround;
            animator.SetBool("LookAround", true);
         
            currentSearchPoint = (currentSearchPoint + 1) % searchPoints.Length;
            if(currentSearchPoint == 0) // if we've looped through all search points
            {
                leaveAfterSearch = true;
                currentSearchPoint = 1; // reset to first search point (skip parent)
            }
          
        }
    }
    public void StartChasing(Transform playerTransform)
    {
        player = playerTransform;
        isChasing = true;

        Debug.Log("Guard started chasing!");

        if (door != null)
        {
            if (!door.isOpen)
                door.ToggleDoor();
        }
    }
    public void ReturnToInitialPosition()
    {
        agent.SetDestination(initialPosition);
        isChasing = false;
        if(Vector3.Distance(transform.position, initialPosition) < 1f)
        {
            currentState = GuardState.Idle;
            animator.SetBool("Walk", false);
            Debug.Log("Guard returned to initial position.");
        }
       
    }
    public void StopChasing()
    {
        isChasing = false;
        agent.ResetPath();

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        if (door != null && door.isOpen)
            door.ToggleDoor();
    }
    public void Detection()
    {
        if(view.Field<Transform>().ToArray().Length != 0)
        {
            Debug.Log("Guard spotted the player in field of view!");
            currentState = GuardState.Chasing;
            animator.SetBool("LookAround", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", true);
        } // check if player is in field of view

       if(player.GetComponent<PlayerMovement>().IsCrouched)
        {
            view.radius = 4f; // reduce view radius when player is crouched
        }
        else
        {
            view.radius = 7f; // reset view radius when player is not crouched
        }
        float DetectDistanceToUse = player.GetComponent<PlayerMovement>().IsCrouched ? DetectDistanceCrouched : DetectDistance; // use reduced distance if player is crouched

        if(Vector3.Distance(transform.position, player.position) < DetectDistanceToUse ) // if player is far away, stop chasing
        {
            Debug.Log("Guard spotted the player!");
                currentState = GuardState.Chasing;
                animator.SetBool("LookAround", false);
                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
        }
    }

    public void CaughtPlayer()
    {
        LoseScreen.SetActive(true); // show lose screen
         Time.timeScale = 0; // pause the game
    }

}
