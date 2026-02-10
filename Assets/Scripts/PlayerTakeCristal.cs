using UnityEngine;
using UnityEngine.Rendering.Universal;


public class PlayerTakeCristal : MonoBehaviour
{
    [SerializeField] private Transform cristalSocket;

    private GameObject nearbyCrystal;
    private GameObject currentCrystal;

    private bool hasbeenTaken = false;

    private bool HoldingCrystal;


    void Start()
    {
        HoldingCrystal = false;
    }

    private void Update()
    {
        if (nearbyCrystal != null && Input.GetKeyDown(KeyCode.E) && !HoldingCrystal)
        {
            Pickup(nearbyCrystal);
            nearbyCrystal = null;
            hasbeenTaken = true;
            HoldingCrystal = true;
        }

        if (hasbeenTaken && Input.GetKeyDown(KeyCode.F))
        {
            // on l'enlève du socket et on le remet dans la scène
            currentCrystal.transform.SetParent(null);
            Rigidbody2D rb = currentCrystal.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true; // ou rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1f; // applique la gravité pour qu'il tombe
                // on lock les mouvements sur l'axe X
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            
            HoldingCrystal = false;
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
        crystal.transform.position = cristalSocket.position;
        crystal.transform.SetParent(cristalSocket);

        Rigidbody2D rb = crystal.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false; // ou rb.bodyType = RigidbodyType2D.Kinematic;

        Collider2D col = crystal.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }
}
