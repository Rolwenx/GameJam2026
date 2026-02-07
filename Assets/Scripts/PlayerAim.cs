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

        // 1️⃣ LASER : qu’est-ce que le faisceau touche ?
        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            direction,
            Mathf.Infinity,
            laserHitMask
        );

        if (hit.collider == null)
        {
            // Rien touché → pas de laser, pas de trajectoire
            if (!isHardLevel)
            {
                trajectoryPrediction.Clear();
                lightBeam.SetActive(false);
            }

            return;
        }
        else
        {
              // Laser visible jusqu’au hit
            Vector3 endPoint = hit.point;
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, endPoint);

        }

        RaycastHit2D hit2 = Physics2D.Raycast(
            origin,
            direction,
            Mathf.Infinity,
            trajectoryMask
        );

        Vector3 endPoint2;
        if (hit2.collider != null)
        {
            if(isHardLevel){
                trajectoryPrediction.Clear();
                Debug.Log(hit2.collider.name);
            }
            else {

            endPoint2 = hit2.point; 

            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, endPoint2);

                // on déclenche la prédiction si ce faisceau touche quelque chose 
            trajectoryPrediction.DrawFromHit(hit2.point, direction, hit2.normal);
            }
        }
        else
        {
            endPoint2 = origin + direction * 10f; 
    
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
