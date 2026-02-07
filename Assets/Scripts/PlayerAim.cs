using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Transform aimTransform;
    public Transform faiseau;
    [SerializeField] private GameObject lightBeam;
    private LineRenderer lineRenderer;
    [SerializeField] private LayerMask laserHitMask;
    [SerializeField] private LayerMask trajectoryMask;

    [Header("Prediction")]
    [SerializeField] private TrajectoryPrediction trajectoryPrediction;
    [SerializeField] private bool isHardLevel;

    


    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        if (lightBeam != null)
        {
            lineRenderer = lightBeam.GetComponentInChildren<LineRenderer>();
            lightBeam.SetActive(false);
        }
    }


    private void Update()
    {
        HandleAiming();
        HandleBeam();
    }

    private void HandleAiming()
    {
        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0,0,angle);
    }

    private void HandleBeam()
    {
        if (Input.GetMouseButton(0))
        {
            if (!lightBeam.activeSelf)
                lightBeam.SetActive(true);
            UpdateLineRenderer();
        }

        if (Input.GetMouseButtonUp(0))
        {
            lightBeam.SetActive(false);
        }
    }

    private void UpdateLineRenderer()
    {
        Vector3 origin = faiseau.position;
        Vector3 direction = (GetMouseWorldPosition() - origin).normalized;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, laserHitMask);

        float maxDist = Mathf.Infinity;

        if (hit.collider == null)
        {
            if (!isHardLevel)
            {
                trajectoryPrediction.Clear();
                lightBeam.SetActive(false);
            }
            return;
        }
        else
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, hit.point);

            maxDist = hit.distance;
        }

        RaycastHit2D hit2 = Physics2D.Raycast(origin, direction, maxDist, trajectoryMask);

        if (hit2.collider != null)
        {
            if (isHardLevel)
            {
                trajectoryPrediction.Clear();
                
                // si l'élement cristal a un tag lit, on passe
                if (!hit2.collider.CompareTag("Lit"))
                {
                    LightCrystal crystal = hit2.collider.GetComponent<LightCrystal>();
                    if (crystal != null)
                    {
                        crystal.Calltolight();
                    }
                }
                
            }
            else
            {
                lineRenderer.SetPosition(0, origin);
                lineRenderer.SetPosition(1, hit2.point);

                trajectoryPrediction.DrawFromHit(hit2.point, direction, hit2.normal);
            }
        }
        else
        {
            if (!isHardLevel)
            {
                trajectoryPrediction.Clear();
            }
        }
    }




    // Get Mouse Position in World with Z = 0f [BY CODEMONKEY]
    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ() {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
