using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Linq;

public class OuverturePorte : MonoBehaviour
{
    [SerializeField] private GameObject porte;
    [SerializeField] private GameObject player;
    public System.Action OnDoorOpened;

    private List<Collider2D> historyHit;

    public bool opened = false;

    private bool sustainUnlocked = false;


    private void Start()
    {
        historyHit = player.GetComponent<PlayerAim>().historyHit;
    }

    private void Update()
    {
        if (opened || porte == null) return;

        HashSet<Transform> neededCristaux = GetLitGrosCristaux(); // cristaux nécessaire pour ouvrir la porte
        HashSet<Transform> touchedSet = GetTouchedGrosCristaux(); // cristaux qui ont été touchés par le rayon du joueur
        Debug.Log($"touchedSet : {string.Join(", ", touchedSet.Select(t => t.name))}"); 
        Debug.Log($"{touchedSet.Count} cristaux touchés");

        if (neededCristaux.Count == 0 || touchedSet.Count == 0) return;

        if (!neededCristaux.SetEquals(touchedSet)) {
            if (touchedSet.Count >= 2)
            {
                Debug.Log("Sustain unlocked!");
                foreach (Transform gros in GetTouchedGrosCristaux())
                {
                    var lc = gros.GetComponent<LightCrystal>();
                    if (lc != null)
                    {
                        lc.lockedOn = true;          // empêche Calltolight de relancer une extinction
                        lc.StopAllCoroutines();
                        // force l'intensité ON
                        // (si tu as ajouté LockOn(), utilise lc.LockOn(5f);)
                    }

                    foreach (var l in gros.GetComponentsInChildren<Light2D>(true))
                        l.intensity = 1f; // mets la valeur que tu veux
                }
            }

        }else {

        opened = true;
        OnDoorOpened?.Invoke();
        porte.SetActive(false);

        foreach (Transform gros in neededCristaux)
        {
            LightCrystal crystal = gros.GetComponent<LightCrystal>();
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
