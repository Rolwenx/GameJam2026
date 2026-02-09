using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Placeable2D : MonoBehaviour
{
    [Header("Collision / Solides")]
    [SerializeField] private LayerMask solidMask;   // sol + murs
    [SerializeField] private float groundRay = 2.0f; // distance max pour trouver un sol sous l’objet

    [Header("Recherche position libre")]
    [SerializeField] private float searchRadius = 1.0f;   // rayon max de recherche autour de la position visée
    [SerializeField] private int searchSteps = 24;         // nb de points testés
    [SerializeField] private float extraSkin = 0.01f;      // petite marge anti-interpénétration

    private Rigidbody2D rb;
    private Collider2D col;

    private Vector2 lastSafePos;
    private bool hasSafePos;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    /// <summary>Appelé quand le joueur porte l’objet.</summary>
    public void SetCarried(bool carried)
    {
        // Pendant qu'on porte : on veut qu’il traverse -> on le rend kinematic
        // et on coupe la gravité. (Tu peux aussi ignorer des layers si tu préfères)
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if (carried)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f; // adapte selon ton jeu
        }
    }

    /// <summary>
    /// Essaie de poser l’objet à targetPos.
    /// - Corrige si ça overlap un mur/sol
    /// - Snap sur le sol si trouvé
    /// - Si pas de sol dessous -> revient à la dernière position safe
    /// </summary>
    public bool TryPlaceAt(Vector2 targetPos)
    {
        // 1) Trouver une position "vide" (pas dans un mur)
        Vector2 freePos;
        if (!FindFreePositionNear(targetPos, out freePos))
        {
            // impossible de trouver une zone libre proche
            return RevertToSafeOrStay();
        }

        // 2) Trouver un sol sous la position candidate (évite de tomber dans le vide)
        Vector2 groundedPos;
        if (!SnapToGround(freePos, out groundedPos))
        {
            // pas de sol sous l’objet -> on annule / on revient à safe
            return RevertToSafeOrStay();
        }

        // 3) Appliquer
        transform.position = groundedPos;

        // Mettre à jour la dernière position sûre
        lastSafePos = groundedPos;
        hasSafePos = true;

        return true;
    }

    /// <summary>À appeler régulièrement quand l’objet est "au sol" pour mémoriser une position sûre.</summary>
    public void MarkSafeIfGrounded()
    {
        // petite vérif : un sol sous l’objet
        if (SnapToGround((Vector2)transform.position, out var grounded))
        {
            lastSafePos = grounded;
            hasSafePos = true;
        }
    }

    private bool FindFreePositionNear(Vector2 desired, out Vector2 freePos)
    {
        // Taille du collider en world
        Bounds b = col.bounds;
        Vector2 size = b.size;

        // Fonction overlap (si quelque chose de "solid" touche la box)
        bool IsOverlapping(Vector2 p)
        {
            // OverlapBox inclut souvent soi-même; on filtre.
            var hit = Physics2D.OverlapBox(p, size, 0f, solidMask);
            if (hit == null) return false;
            if (hit == col) return false;
            return true;
        }

        // 1) test direct
        if (!IsOverlapping(desired))
        {
            freePos = desired;
            return true;
        }

        // 2) recherche autour en “spirale” (cercle discret)
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

    private bool SnapToGround(Vector2 pos, out Vector2 groundedPos)
    {
        // On raycast depuis un point légèrement au-dessus du bas du collider
        Bounds b = col.bounds;
        float halfH = b.extents.y;
        float halfW = b.extents.x;

        // Origine du ray : au centre X, juste au-dessus du bas
        Vector2 origin = new Vector2(pos.x, pos.y + 0.01f);

        // Petit trick : on fait 3 rays (centre, gauche, droite) pour être stable sur les bords
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

        // Position finale : posé sur le sol (point du sol + demi-hauteur)
        float y = best.point.y + halfH + extraSkin;
        groundedPos = new Vector2(pos.x, y);
        return true;
    }

    private bool RevertToSafeOrStay()
    {
        if (hasSafePos)
        {
            transform.position = lastSafePos;
            return false;
        }

        // Pas de safe position connue -> on ne bouge pas
        return false;
    }

    // Debug visuel de l’OverlapBox (optionnel)
    void OnDrawGizmosSelected()
    {
        if (!col) col = GetComponent<Collider2D>();
        if (!col) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, col.bounds.size);
    }
}
