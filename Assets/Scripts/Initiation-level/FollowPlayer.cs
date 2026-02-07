using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerMovement target;

    [Header("Follow")]
    [SerializeField] private float followDelay = 0.15f;
    [SerializeField] private Vector2 followOffset = new Vector2(-1.2f, 0f);

    [Header("Move")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 8f;
    [Header("Teleport")]
    [SerializeField] private float maxDistanceFromPlayer = 8f;
    [SerializeField] private float teleportDistance = 1.5f;

    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer sr;

    // buffered state (delay)
    private float bufferedX;
    private bool bufferedRun;
    private bool bufferedJump;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        body.freezeRotation = true;
    }

    private void Update()
    {
        if (!target) return;

        // ---- BUFFER INPUT (DELAY FEEL) ----
        bufferedX = Mathf.Lerp(bufferedX, target.CurrentX, Time.deltaTime / followDelay);
        bufferedRun = target.IsRunning;

        if (target.JumpTriggered)
            bufferedJump = true;

        // ---- ANIM ----
        animator.SetFloat("Speed", target.CurrentSpeed);
        animator.SetBool("IsGrounded", target.IsGroundedPublic);

        if (bufferedX != 0)
            sr.flipX = bufferedX < 0;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        HandleTeleport(); // <- HERE

        float speed = bufferedRun ? runSpeed : walkSpeed;

        body.linearVelocity = new Vector2(
            bufferedX * speed,
            body.linearVelocity.y
        );

        if (bufferedJump && target.IsGroundedPublic)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0f);
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
            bufferedJump = false;
        }
    }

    private void HandleTeleport()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);

        if (distance > maxDistanceFromPlayer)
        {
            // Choose side based on player facing direction
            float side = target.CurrentX != 0 
                ? Mathf.Sign(target.CurrentX) 
                : (sr.flipX ? -1 : 1);

            Vector2 teleportPos = (Vector2)target.transform.position 
                                + new Vector2(-side * teleportDistance, 0f);

            body.linearVelocity = Vector2.zero; // critical!
            body.position = teleportPos;
        }
    }
}