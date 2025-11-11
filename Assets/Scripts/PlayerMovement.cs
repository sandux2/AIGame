using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Rotation
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Move player 
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //Jump
      if (controller.isGrounded && Input.GetButtonDown("Jump"))
{
    animator.ResetTrigger("Jump");    // prevent overlap
    animator.SetTrigger("Jump");      // play jump anim
    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
}


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
{
    Debug.Log("JUMP PRESSED - Grounded = " + controller.isGrounded);
    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    animator.SetTrigger("Jump");
}


        //Animation
        animator.SetFloat("Speed", move.magnitude);
    }
}
