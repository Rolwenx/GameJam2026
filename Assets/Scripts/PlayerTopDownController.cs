using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerTopDownController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Input filtering")]
    [SerializeField] private float deadzone = 0.10f; // augmente si tu vois encore un léger drift

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

        // Top-down: pas de gravité + mouvement "propre"
        rb.gravityScale = 0f;
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
        // Sécurité: si pas d'input -> vélocité EXACTEMENT à zéro
        if (moveInput == Vector2.zero)
            rb.linearVelocity = Vector2.zero;
        else
            rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();

        // Deadzone pour éviter les micro-valeurs qui font "glisser"
        if (v.sqrMagnitude < deadzone * deadzone)
            v = Vector2.zero;

        // Si tu veux garder une vitesse constante en diagonale:
        moveInput = v == Vector2.zero ? Vector2.zero : v.normalized;
    }
}
