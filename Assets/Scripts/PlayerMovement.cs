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
    [SerializeField] private BoxCollider2D feetBox;          
    [SerializeField] private float groundCastDistance = 0.05f;

    [Header("Better Fall")]
    [SerializeField] private float fallMultiplier = 2f; // 1.5–2.5 conseillé

    [Header("Anti Wall-Stick")]
    [SerializeField] private Transform wallCheck;           // point au niveau du torse
    [SerializeField] private float wallCheckDistance = 0.08f;
    [SerializeField] private LayerMask wallLayer;           // souvent = groundLayer
    [SerializeField] private float airWallPushDampen = 0.1f; // 0 = stop net, 0.1 = léger push

    [SerializeField] private Vector2 socketOffsetRight = new Vector2(0.35f, 0.1f);
    [SerializeField] private Transform socketCristal;

    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer sr;

    private float x;
    private bool isRunning;
    private bool jumpPressed;

    public AudioSource walkingAudioSource, jumpingAudioSource, runningAudioSource;

    

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        body.freezeRotation = true;

        // si tu ne veux pas gérer 2 layers, tu peux faire:
        // wallLayer = groundLayer; (mais wallLayer doit être assigné dans l'inspecteur si tu fais ça pas)
    }

    private void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;

        if (x != 0) sr.flipX = x < 0;


        // Socket toujours devant
        if (socketCristal != null)
        {
            bool facingLeft = sr.flipX; // true = gauche
            Vector2 off = socketOffsetRight;
            off.x = facingLeft ? -Mathf.Abs(off.x) : Mathf.Abs(off.x);
            socketCristal.localPosition = off;
        }

        float animSpeed = 0f;
        if (Mathf.Abs(x) > 0.01f)
            animSpeed = isRunning ? 1f : 0.5f;

        animator.SetFloat("Speed", animSpeed);
    }

    private void FixedUpdate()
    {
        bool isGrounded = IsGrounded();
        animator.SetBool("IsGrounded", isGrounded);

        bool isMoving = Mathf.Abs(x) > 0.01f;

        if (isGrounded && isMoving)
        {
            if (isRunning)
            {
                if (walkingAudioSource != null && walkingAudioSource.isPlaying) walkingAudioSource.Stop();
                if (runningAudioSource != null && !runningAudioSource.isPlaying) runningAudioSource.Play();
            }
            else
            {
                if (runningAudioSource != null && runningAudioSource.isPlaying) runningAudioSource.Stop();
                if (walkingAudioSource != null && !walkingAudioSource.isPlaying) walkingAudioSource.Play();
            }
        }
        else
        {
            if (walkingAudioSource != null && walkingAudioSource.isPlaying) walkingAudioSource.Stop();
            if (runningAudioSource != null && runningAudioSource.isPlaying) runningAudioSource.Stop();
        }


        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // applique le mouvement horizontal
        body.linearVelocity = new Vector2(x * currentSpeed, body.linearVelocity.y);

        // jump
        if (jumpPressed && isGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0f);
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
            if (walkingAudioSource != null && walkingAudioSource.isPlaying) walkingAudioSource.Stop();
            if (runningAudioSource != null && runningAudioSource.isPlaying) runningAudioSource.Stop();

            jumpingAudioSource.Play();
        }
        jumpPressed = false;

        // chute plus "snappy"
        if (body.linearVelocity.y < 0f)
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }

        // anti wall-stick (si en l'air et collé à un mur, on réduit fortement la vitesse X)
        if (!isGrounded && IsTouchingWall() && body.linearVelocity.y <= 0f)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x * airWallPushDampen, body.linearVelocity.y);
        }
    }

    private bool IsTouchingWall()
    {
        if (!wallCheck) return false;

        RaycastHit2D right = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, wallLayer);
        RaycastHit2D left  = Physics2D.Raycast(wallCheck.position, Vector2.left,  wallCheckDistance, wallLayer);

        return right.collider != null || left.collider != null;
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
        // feet cast
        if (feetBox)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = feetBox.bounds.center + Vector3.down * groundCastDistance;
            Gizmos.DrawWireCube(center, feetBox.bounds.size);
        }

        // wall rays
        if (wallCheck)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.left * wallCheckDistance);
        }
    }

    public float CurrentX => x;
    public bool IsRunning => isRunning;
    public bool JumpTriggered => jumpPressed;
    public bool IsGroundedPublic => IsGrounded();
    public float CurrentSpeed =>
        Mathf.Abs(x) < 0.01f ? 0f : (isRunning ? 1f : 0.5f);
}
    