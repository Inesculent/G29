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

    private Rigidbody rb;

    void Start()
    {
        // Try to grab components if not set via inspector.
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Determine if the player is moving based on input.
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = new Vector2(horizontal, vertical).magnitude > moveThreshold;

        // Check if the player is grounded.
        bool isGrounded = false;
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        }
        else if (rb != null)
        {
            // Fallback to a simple velocity check if no groundCheck transform is set.
            isGrounded = Mathf.Abs(rb.velocity.y) < 0.1f;
        }

        // Detect jump input: Here we assume a jump starts when the player presses the jump button and is grounded.
        bool isJumping = Input.GetButtonDown("Jump") && isGrounded;

        // Set animator flags accordingly.
        animator.SetBool("Jump", isJumping);
        animator.SetBool("Moving", isMoving);
        animator.SetBool("Grounded", isGrounded);
    }
}