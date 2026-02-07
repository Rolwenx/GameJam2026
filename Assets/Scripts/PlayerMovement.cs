using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Ground Check (Feet BoxCast)")]
    [SerializeField] private BoxCollider2D feetBox;          // BoxCollider2D aplati sous les pieds
    [SerializeField] private float groundCastDistance = 0.05f;

    [Header("Better Jump")]
    [SerializeField] private float fallMultiplier = 3f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer sr;

    // input cache
    private float x;
    private bool isRunning;
    private bool jumpPressed;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // optionnel mais conseillé
        body.freezeRotation = true;
    }

    private void Update()
    {
        // Inputs
        x = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;

        // Anim + Flip (ok en Update)
        if (x != 0) sr.flipX = x < 0;

        float animSpeed = 0f;
        if (Mathf.Abs(x) > 0.01f)
            animSpeed = isRunning ? 1f : 0.5f;

        animator.SetFloat("Speed", animSpeed);
    }

    private void FixedUpdate()
    {
        bool isGrounded = IsGrounded();
        animator.SetBool("IsGrounded", isGrounded);

        // Horizontal move
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        body.linearVelocity = new Vector2(x * currentSpeed, body.linearVelocity.y);

        // Jump
        if (jumpPressed && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
        jumpPressed = false;

        // Better jump feel
        if (body.linearVelocity.y < 0)
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (body.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private bool IsGrounded()
    {
        if (!feetBox) return false;

        RaycastHit2D hit = Physics2D.BoxCast(
            feetBox.bounds.center,
            feetBox.bounds.size,
            0f,
            Vector2.down,
            groundCastDistance,
            groundLayer
        );

        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (!feetBox) return;

        Gizmos.color = Color.yellow;
        Vector3 center = feetBox.bounds.center + Vector3.down * groundCastDistance;
        Gizmos.DrawWireCube(center, feetBox.bounds.size);
    }
}
