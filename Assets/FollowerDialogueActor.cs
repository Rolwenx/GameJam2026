using UnityEngine;

public class FollowerDialogueActor : MonoBehaviour
{
    [SerializeField] private float distanceFromPlayer = 2.5f;

    private Transform player;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false); // IMPORTANT
    }

    public void ShowNearPlayer(Transform playerTransform)
    {
        player = playerTransform;

        Vector3 sideOffset = player.localScale.x >= 0
            ? Vector3.right
            : Vector3.left;

        transform.position = player.position + sideOffset * distanceFromPlayer;

        LookAtPlayer();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void LookAtPlayer()
    {
        if (!player || !sr) return;
        sr.flipX = transform.position.x > player.position.x;
    }
}