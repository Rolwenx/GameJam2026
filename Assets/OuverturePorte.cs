using UnityEngine;
using UnityEngine.Rendering.Universal;


public class OuverturePorte : MonoBehaviour
{
    [SerializeField] private GameObject porte;

    private bool opened = false;

    void Update()
    {
        if (opened) return;

        bool allLit = true;

        foreach (Transform child in transform)
        {
            if (porte != null && child.gameObject == porte)
                continue;

            if (!child.CompareTag("Lit"))
            {
                allLit = false;
                break;
            }
        }

        if (allLit && porte != null)
        {
            opened = true;

            // Ouvre la porte
            porte.SetActive(false);

            // Désactive LightCrystal sur tous les enfants sauf "porte"
            foreach (Transform child in transform)
            {
                if (porte != null && child.gameObject == porte)
                    continue;

                LightCrystal crystal = child.GetComponent<LightCrystal>();
                if (crystal != null)
                {
                    crystal.CancelAll();
                    crystal.enabled = false;
                }
                foreach(Transform grandChild in child)
                {
                    Light2D light2D = grandChild.GetComponent<Light2D>();
                    if (light2D != null)
                    {
                        light2D.intensity = 2f; // ou une autre valeur pour "allumé"
                    }
                }
            }
        }
    }

}
