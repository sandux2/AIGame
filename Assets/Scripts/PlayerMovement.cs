using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 200f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private CharacterController controller; // reference to CharacterController
    private Animator animator;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>(); // get CharacterController component
        animator = GetComponentInChildren<Animator>();    // get Animator from child objects
    }

    void Update()
    {
        float moveZ = Input.GetAxis("Vertical"); // W/S for movement
        float rotateX = Input.GetAxis("Horizontal"); // A/D for rotation

        // Rotation
        transform.Rotate(Vector3.up * rotateX * rotationSpeed * Time.deltaTime);

        //Forward/backward movement (only W/S) 
        Vector3 move = transform.forward * moveZ;

        // Jump and gravity
        if (controller.isGrounded)
        {
            if (velocity.y < 0) velocity.y = -2f;

            if (Input.GetButtonDown("Jump"))
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
        if (animator)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveZ)); 
        }
    }
}