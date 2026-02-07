using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Transform aimTransform;
    public Transform faiseau;
    [SerializeField] private GameObject lightBeam;
    private LineRenderer lineRenderer;
    [SerializeField] private float maxDistance = 10f;

    [Header("Prediction")]
    [SerializeField] private TrajectoryPrediction trajectoryPrediction;

    


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

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity);

        Vector3 endPoint;

        if (hit.collider != null)
        {
            endPoint = hit.point;
             // on déclenche la prédiction si ce faisceau touche quelque chose
            trajectoryPrediction.DrawFromHit(hit.point, direction, hit.normal);
        }
        else
        {
            endPoint = origin + direction * maxDistance;
            trajectoryPrediction.Clear();
        }

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, endPoint);

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
