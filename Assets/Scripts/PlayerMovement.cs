using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private Transform isItGround;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private float xInput;
    private bool jumpPressed;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
    }

    private void Update()
    {
        // movement
        xInput = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;

        // check if char on ground
        isGrounded = Physics2D.OverlapCircle(isItGround.position, groundCheckRadius, groundLayer);

        // Flip
        if (xInput != 0)
            sr.flipX = xInput < 0;

        anim.SetFloat("Speed", Mathf.Abs(xInput));
        anim.SetBool("IsGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        // Move
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);

        // Jump
        if (jumpPressed && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        jumpPressed = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (isItGround == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(isItGround.position, groundCheckRadius);
    }
}