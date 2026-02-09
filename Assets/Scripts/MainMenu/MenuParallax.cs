using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = .3f;

    private Vector3 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // viewport: (0..1). On recentre pour avoir (-0.5..+0.5)
        Vector2 viewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 centered = viewport - new Vector2(0.5f, 0.5f);

        // optionnel: limiter un peu l'amplitude si tu veux
        // centered = Vector2.ClampMagnitude(centered, 0.5f);

        Vector3 target = startPosition + new Vector3(centered.x, centered.y, 0f) * offsetMultiplier;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            target,
            ref velocity,
            smoothTime
        );
    }
}
