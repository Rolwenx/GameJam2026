using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Linq;

public class OuverturePorte : MonoBehaviour
{
    [SerializeField] private GameObject porte;
    [SerializeField] private GameObject player;

    private List<Collider2D> historyHit;

    public bool opened = false;

    private void Start()
    {
        historyHit = player.GetComponent<PlayerAim>().historyHit;
    }

    private void Update()
    {
        if (opened || porte == null) return;

        HashSet<Transform> neededCristaux = GetLitGrosCristaux(); // cristaux nécessaire pour ouvrir la porte
        HashSet<Transform> touchedSet = GetTouchedGrosCristaux(); // cristaux qui ont été touchés par le rayon du joueur

        if (neededCristaux.Count == 0 || touchedSet.Count == 0) return;

        if (!neededCristaux.SetEquals(touchedSet)) return; // on vérifie si la liste des cristaux touchées correspond à la liste des cristaux qui doivent être allumés

        opened = true;
        porte.SetActive(false);

        foreach (Transform gros in neededCristaux)
        {
            LightCrystal crystal = gros.GetComponent<LightCrystal>();
            Debug.Log($"Sur {gros.name} -> LightCrystal trouvé ? {gros.GetComponentInChildren<LightCrystal>(true) != null}");

            if (crystal != null)
            {
                crystal.CancelAll();
                crystal.enabled = false;
            }

            foreach (Transform child in gros)
            {
                Light2D light2D = child.GetComponent<Light2D>();
                if (light2D != null) light2D.intensity = 2f;
            }
        }
    }


    private HashSet<Transform> GetLitGrosCristaux()
    {
        var set = new HashSet<Transform>();

        foreach (Transform child in transform)
        {
            if (porte != null && child.gameObject == porte) continue;
            if (!child.name.Contains("GrosCristaux")) continue;
                set.Add(child);
        }

        return set;
    }

    private HashSet<Transform> GetTouchedGrosCristaux()
    {
        var set = new HashSet<Transform>();
        if (historyHit == null) return set;

        foreach (var col in historyHit)
        {
            if (col == null) continue;

            Transform t = col.transform;
            while (t != null && t.parent != transform)
                t = t.parent;

            if (t == null) continue;
            if (porte != null && t.gameObject == porte) continue;
            if (!t.name.Contains("GrosCristaux")) continue;

            set.Add(t);
        }

        return set;
    }
}
