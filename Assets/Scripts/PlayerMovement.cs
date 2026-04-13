using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f, runningSpeed = 8f, walkingSpeed = 5f;
    public float rotationSpeed = 200f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public bool IsCrouched = false;
    public bool isRunning = false;
     public AudioSource footstepSound; // sound effect for footsteps when player is walking
    private CharacterController controller; // reference to CharacterController
    private Animator animator;
    private Vector3 velocity;
    public AudioClip walkingClip, runningClip; // sound effect for footsteps when player is walking

    void Start()
    {
        controller = GetComponent<CharacterController>(); // get CharacterController component
        animator = GetComponentInChildren<Animator>();    // get Animator from child objects
    }

    void Update()
    {
        float moveZ = Input.GetAxis("Vertical"); // W/S for movement
        float rotateX = Input.GetAxis("Horizontal"); // A/D for rotation
        
        animator.SetBool("IsRunning", isRunning); // set running animation
        // Rotation
        transform.Rotate(Vector3.up * rotateX * rotationSpeed * Time.deltaTime);

        //Forward/backward movement (only W/S) 
        Vector3 move = transform.forward * moveZ;

        // Jump and gravity
        if (controller.isGrounded )
        {
            if (velocity.y < 0) velocity.y = -2f;

            if (Input.GetButtonDown("Jump") && ! IsCrouched)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                if (animator) animator.SetTrigger("Jump");
            }
        }

        // Apply movement
        controller.Move(move * speed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Animation
        animator.SetBool("IsCrouched", IsCrouched); // set crouch animation
        if (animator)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveZ)); 
        }

        // Crouch toggle
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(IsCrouched)
            {
                
                IsCrouched = false;
            }
            else
            {
            
                IsCrouched = true;
            }
        }
       
       
       if(Input.GetKey(KeyCode.LeftShift)&& !IsCrouched)
        {
            isRunning = true;
             speed = runningSpeed;
             footstepSound.clip = runningClip;
             footstepSound.volume = 1f; // increase volume for running footsteps
        }
        else
        {
            isRunning = false;
            speed = walkingSpeed;
            footstepSound.clip = walkingClip;
            footstepSound.volume = 0.2f; // increase volume for running footsteps
        }

        if (moveZ != 0 && controller.isGrounded) // play footstep sound when moving on ground
        {
            if (!footstepSound.isPlaying)
            {
                footstepSound.Play();
            }
        }
        else
        {
            footstepSound.Stop();
        }
    }
    
}