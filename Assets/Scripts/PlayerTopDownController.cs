using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTopDownController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.down; // default facing

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // Store last direction ONLY if moving
        if (moveInput != Vector2.zero)
            lastDirection = moveInput;

        Vector2 animDirection = (moveInput == Vector2.zero) 
            ? lastDirection 
            : moveInput;

        animator.SetFloat("MoveX", animDirection.x);
        animator.SetFloat("MoveY", animDirection.y);
        animator.SetFloat("Speed", moveInput.sqrMagnitude);

        // Flip ONLY when moving sideways
        if (animDirection.x != 0)
            sr.flipX = animDirection.x < 0;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
    }
}