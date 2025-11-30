using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public Transform cameraTransform;
    public LayerMask groundMask;

    private Rigidbody rb;
    private bool isGrounded;
    private int currentJumps = 0;
    private int maxJumps = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Auto-find camera if not assigned
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Jump Input
        if (isGrounded)
        {
            currentJumps = 0;   // reset jumps when touching ground
        }

        if (Input.GetKeyDown(KeyCode.Space) && currentJumps < maxJumps)
        {
            HandleJump();
            currentJumps++;
        }

        HandleMovement();
        GroundCheck();
    }

    void HandleMovement()
    {
        // Get input axes
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v).normalized;
        // Only move if there's input
        if (inputDir.magnitude >= 0.1f)
        {
            // Make movement relative to camera
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            // Project onto horizontal plane
            camForward.y = 0;
            camRight.y = 0;
            // Normalize
            camForward.Normalize();
            camRight.Normalize();
            // Calculate move direction
            Vector3 moveDir = (camForward * v + camRight * h).normalized;
            // Move player
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);

            // Rotate player toward direction
            transform.forward = moveDir;
        }
    }

    void HandleJump()
    {
        // Jump if grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
    }
}