using UnityEngine;

public sealed class OnFootMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float acceleration = 25f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5.5f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private bool isGrounded;

    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    public void Move(Vector2 input, bool sprint)
    {
        Vector3 moveDir =
            transform.forward * input.y +
            transform.right * input.x;

        float targetSpeed = sprint ? sprintSpeed : walkSpeed;
        Vector3 targetVelocity = moveDir.normalized * targetSpeed;

        Vector3 velocity = rb.linearVelocity;
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0f, velocity.z);

        rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);
    }

    public void Jump()
    {
        if (!isGrounded) return;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        isGrounded = Physics.Raycast(
            origin,
            Vector3.down,
            groundCheckDistance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        Debug.DrawRay(origin, Vector3.down * groundCheckDistance,
            isGrounded ? Color.green : Color.red);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.1f, 0.25f);
    }
}