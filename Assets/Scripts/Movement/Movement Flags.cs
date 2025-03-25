using UnityEngine;

public class MovementFlags : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Ground-check parameters
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundMask;

    // Movement threshold
    [SerializeField] private float moveThreshold = 0.1f;

    // Optional: Use CharacterController for backup grounded check
    [SerializeField] private CharacterController characterController;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = new Vector2(horizontal, vertical).magnitude > moveThreshold;

        // Check if the player is grounded
        bool isGrounded = false;
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        }
        else if (characterController != null)
        {
            isGrounded = characterController.isGrounded;
        }

        // Detect jump input
        bool isJumping = Input.GetButtonDown("Jump") && isGrounded;

        animator.SetBool("Jump", isJumping);
        animator.SetBool("Moving", isMoving);
        animator.SetBool("Grounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.F)) // Punch
        {
            animator.SetBool("Crouch", false);
            animator.SetTrigger("Punch");
        }
    }
}
