using UnityEngine;

public class PlayerTakeCristal : MonoBehaviour
{
    [Header("Socket")]
    [SerializeField] private Transform cristalSocket;

    [Header("Pick/Drop")]
    [SerializeField] private LayerMask solidMask;     // mets ici ton layer Sol/Murs
    [SerializeField] private float groundRay = 3f;    // distance max pour trouver un sol sous l’objet
    [SerializeField] private float searchRadius = 1f; // rayon de recherche si ça overlap un mur
    [SerializeField] private int searchSteps = 24;    // nb de tests autour

    private GameObject nearbyCrystal;
    private GameObject currentCrystal;

    private bool hasbeenTaken = false;

    // dernière position valide “au sol”
    private Vector2 lastSafeCrystalPos;
    private bool hasSafePos = false;

    private void Update()
    {
        if (nearbyCrystal != null && Input.GetKeyDown(KeyCode.E))
        {
            Pickup(nearbyCrystal);
            nearbyCrystal = null;
            hasbeenTaken = true;
        }

        // optionnel : pendant qu’on porte, on garde l’objet exactement sur le socket
        if (hasbeenTaken && currentCrystal != null)
        {
            currentCrystal.transform.position = cristalSocket.position;
        }

        if (hasbeenTaken && Input.GetKeyDown(KeyCode.F))
        {
            DropSafely();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("CristalsToTake"))
        {
            nearbyCrystal = collision.gameObject;
            Debug.Log("Appuie sur E pour ramasser");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == nearbyCrystal)
            nearbyCrystal = null;
    }

    private void Pickup(GameObject crystal)
    {
        currentCrystal = crystal;

        // mémoriser une position safe au moment du pickup (si possible)
        TryMarkSafe(currentCrystal);

        crystal.transform.position = cristalSocket.position;
        crystal.transform.SetParent(cristalSocket);

        var rb = crystal.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false; // traverse tout pendant qu’on porte
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        var col = crystal.GetComponent<Collider2D>();
        if (col != null) col.enabled = false; // traverse tout pendant qu’on porte
    }

    private void DropSafely()
    {
        if (currentCrystal == null) return;

        // détacher
        currentCrystal.transform.SetParent(null);

        var rb = currentCrystal.GetComponent<Rigidbody2D>();
        var col = currentCrystal.GetComponent<Collider2D>();

        if (col != null) col.enabled = true;
        if (rb != null)
        {
            rb.simulated = true;
            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // 1) trouver une position libre proche du socket
        Vector2 freePos;
        if (!FindFreePositionNear(cristalSocket.position, currentCrystal, out freePos))
        {
            // impossible => retour safe
            RevertToSafe();
            FinishDrop();
            return;
        }

        // 2) snap sur le sol sous cette position (évite le vide)
        Vector2 groundedPos;
        if (!SnapToGround(freePos, currentCrystal, out groundedPos))
        {
            // pas de sol => retour safe
            RevertToSafe();
            FinishDrop();
            return;
        }

        currentCrystal.transform.position = groundedPos;

        // 3) mise à jour safe
        lastSafeCrystalPos = groundedPos;
        hasSafePos = true;

        FinishDrop();
    }

    private void FinishDrop()
    {
        hasbeenTaken = false;
        currentCrystal = null;
    }

    private void RevertToSafe()
    {
        if (currentCrystal == null) return;

        if (hasSafePos)
        {
            currentCrystal.transform.position = lastSafeCrystalPos;
        }
        // sinon : on laisse où c’est (mais normalement tu auras une safe pos)
    }

    private bool FindFreePositionNear(Vector2 desired, GameObject obj, out Vector2 freePos)
    {
        var col = obj.GetComponent<Collider2D>();
        if (col == null)
        {
            freePos = desired;
            return true;
        }

        Vector2 size = col.bounds.size;

        bool IsOverlapping(Vector2 p)
        {
            var hit = Physics2D.OverlapBox(p, size, 0f, solidMask);
            if (hit == null) return false;
            if (hit == col) return false;
            return true;
        }

        // test direct
        if (!IsOverlapping(desired))
        {
            freePos = desired;
            return true;
        }

        // recherche autour
        for (int i = 1; i <= searchSteps; i++)
        {
            float t = (float)i / searchSteps;
            float angle = t * Mathf.PI * 2f;
            float r = t * searchRadius;

            Vector2 candidate = desired + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;
            if (!IsOverlapping(candidate))
            {
                freePos = candidate;
                return true;
            }
        }

        freePos = desired;
        return false;
    }

    private bool SnapToGround(Vector2 pos, GameObject obj, out Vector2 groundedPos)
    {
        var col = obj.GetComponent<Collider2D>();
        if (col == null)
        {
            groundedPos = pos;
            return true;
        }

        float halfH = col.bounds.extents.y;
        float halfW = col.bounds.extents.x;

        Vector2 origin = new Vector2(pos.x, pos.y + 0.02f);

        Vector2[] origins =
        {
            origin,
            origin + Vector2.left * (halfW * 0.6f),
            origin + Vector2.right * (halfW * 0.6f)
        };

        RaycastHit2D best = default;
        bool found = false;

        foreach (var o in origins)
        {
            var hit = Physics2D.Raycast(o, Vector2.down, groundRay, solidMask);
            if (hit.collider == null) continue;

            if (!found || hit.distance < best.distance)
            {
                best = hit;
                found = true;
            }
        }

        if (!found)
        {
            groundedPos = pos;
            return false;
        }

        groundedPos = new Vector2(pos.x, best.point.y + halfH + 0.01f);
        return true;
    }

    private void TryMarkSafe(GameObject obj)
    {
        // tente de trouver un sol sous l’objet et mémoriser
        Vector2 grounded;
        if (SnapToGround(obj.transform.position, obj, out grounded))
        {
            lastSafeCrystalPos = grounded;
            hasSafePos = true;
        }
    }
}
