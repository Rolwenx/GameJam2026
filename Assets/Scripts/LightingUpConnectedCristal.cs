using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class LightingUpConnectedCristal : MonoBehaviour
{
    public PlayerAim playerAim;

    void LateUpdate()
    {
        if (playerAim == null) return;

        var hits = playerAim.historyHit;
        Debug.Log("Hits dans LightingUpConnectedCristal : " + string.Join(", ", hits.Select(c => c.name)));
        if (hits == null || hits.Count == 0) return;

        if (hits.Count >= 2){
            foreach (var gros in hits
                    .Where(t => t.name.Contains("GrosCristaux"))
                    .Distinct()){
                // le script est sur un enfant, pas sur gros
                var crystal = gros.GetComponent<LightCrystal>();

                if (crystal != null)
                {
                    crystal.CancelAll();
                    //crystal.enabled = false; // attention: ça le coupe définitivement tant que tu ne le réactives pas
                }

                foreach (Transform child in gros.transform)
                {
                    var light2D = child.GetComponent<Light2D>();
                    if (light2D != null) light2D.intensity = 0.7f;
                }
            }
        }
        else {
            return;
        }
    }
}
