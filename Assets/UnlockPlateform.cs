using UnityEngine;

public class UnlockPlateform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject plateformHidden;
    [SerializeField] private bool firstimeOnplateform = true;
    public GameObject porte4;
    private OuverturePorte ouverturePorte;
    public bool goUp = false;

    void Start()
    {
        plateformHidden.SetActive(false);
        ouverturePorte = porte4.GetComponent<OuverturePorte>();

    }

    void Update()
    {
        ouverturePorte = porte4.GetComponent<OuverturePorte>();
        if (ouverturePorte.opened && goUp)
        {
            Up();
        }
    }
    

    public void Up(){
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 18f, transform.position.z), 5f * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && firstimeOnplateform)
        {
            firstimeOnplateform = false;
            plateformHidden.SetActive(true);
        }
        else {
            goUp = true;

        }
    }
}
