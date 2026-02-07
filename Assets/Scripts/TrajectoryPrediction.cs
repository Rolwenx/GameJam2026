using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPrediction : MonoBehaviour
{
    [SerializeField] private float maxDistance = 15f;
    [SerializeField] private int maxBounces = 1;
    [SerializeField] private LayerMask reflectLayer;

    [SerializeField] private LineRenderer trajectory_line;

    [System.Serializable]
    public struct BounceHit
    {
        public Collider2D collider;
        public Vector2 point;
        public Vector2 normal;
    }

    // Historique des impacts (rebonds) calculés lors du dernier DrawFromHit()
    public readonly List<BounceHit> History = new List<BounceHit>();

    public void DrawFromHit(Vector2 hitPoint, Vector2 incomingDir, Vector2 hitNormal)
    {
        History.Clear();

        trajectory_line.positionCount = 1;
        trajectory_line.SetPosition(0, hitPoint);

        Vector2 startPos = hitPoint + hitNormal * 0.01f;
        Vector2 direction = Vector2.Reflect(incomingDir, hitNormal);

        int points = 1;

        for (int i = 0; i < maxBounces + 1; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(startPos, direction, maxDistance, reflectLayer);

            if (hit.collider != null)
            {
                // ✅ stocke l'historique du rebond
                History.Add(new BounceHit
                {
                    collider = hit.collider,
                    point = hit.point,
                    normal = hit.normal
                });

                if (!hit.collider.CompareTag("Lit"))
                {
                    LightCrystal crystal = hit.collider.GetComponent<LightCrystal>();
                    if (crystal != null)
                    {
                        crystal.Calltolight(false);
                    }
                }


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
        History.Clear();
        trajectory_line.positionCount = 0;
    }
}
