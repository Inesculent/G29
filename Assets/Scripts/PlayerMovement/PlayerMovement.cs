using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    
    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dashAction;

    private Vector2 movementInput;
    private bool isDashing = false;
    private float dashTimer;
    private float lastDashTime = -Mathf.Infinity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["move"];
        dashAction = playerInput.actions["dash"];
    }

    private void OnEnable()
    {
        moveAction.Enable();
        dashAction.Enable();
    }
    
    private void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
    }

    private void Update()
    {
        // Only update movement input if not dashing
        if (!isDashing)
            movementInput = moveAction.ReadValue<Vector2>();

        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashDuration)
                isDashing = false;
        }
    }

    private void FixedUpdate()
    {
        // Convert 2D input to a 3D direction relative to the player's orientation
        Vector3 direction = transform.right * movementInput.x + transform.forward * movementInput.y;
        direction = direction.normalized;

        if (isDashing)
            rb.velocity = direction * dashSpeed;
        else
            rb.velocity = direction * movementSpeed;
    }

        public void OnDash()
        {
            if (Time.time >= lastDashTime + dashCooldown && movementInput != Vector2.zero)
            {
                isDashing = true;
                dashTimer = 0f;
                lastDashTime = Time.time;
            }
        }
    }
