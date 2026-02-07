using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPrediction : MonoBehaviour
{
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private int maxBounces = 1;
    [SerializeField] private LayerMask reflectLayer;

    [SerializeField] private LineRenderer trajectory_line;


    public void DrawFromHit(Vector2 hitPoint, Vector2 incomingDir, Vector2 hitNormal)
    {
        trajectory_line.positionCount = 1;
        trajectory_line.SetPosition(0, hitPoint);

        Vector2 startPos = hitPoint + hitNormal * 0.01f;
        Vector2 direction = Vector2.Reflect(incomingDir, hitNormal);

        int points = 1;

        for (int i = 0; i < maxBounces + 1; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(startPos, direction, maxDistance, reflectLayer);

            if (hit)
            {
                trajectory_line.positionCount++;
                trajectory_line.SetPosition(points, hit.point);
                points++;

                startPos = hit.point + hit.normal * 0.01f;
                direction = Vector2.Reflect(direction, hit.normal);
            }
            else
            {
                trajectory_line.positionCount++;
                trajectory_line.SetPosition(points, startPos + direction * maxDistance);
                break;
            }
        }
    }

    public void Clear()
    {
        trajectory_line.positionCount = 0;
    }
}