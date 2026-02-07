using UnityEngine;
using System.Collections;

public class LightCrystal : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private float onIntensity = 2f;
    [SerializeField] private float offIntensity = 0f;

    [Header("Timings")]
    [SerializeField] private float fadeDuration = 0.12f;   // fade rapide
    [SerializeField] private float stayLitTime = 3f;       // reste allumé

    // Référence au composant de lumière (URP 2D)
    private UnityEngine.Rendering.Universal.Light2D light2D;

    private Coroutine routine;

    private void Awake()
    {
        // 1er enfant = ton light
        Transform light = transform.GetChild(0);
        light2D = light.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        if (light2D == null)
            Debug.LogError("LightCrystal: Aucun composant Light2D trouvé sur le 1er enfant.");

        // On part éteint
        if (light2D != null) light2D.intensity = offIntensity;
    }

    public void Calltolight()
    {
        if (light2D == null) return;

        // Evite de superposer plusieurs coroutines
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(LightUp());
    }

    private IEnumerator LightUp()
    {
        // Fade in -> intensity 2
        yield return FadeIntensity(offIntensity, onIntensity, fadeDuration);

        // Reste allumé
        yield return new WaitForSeconds(stayLitTime);

        // Fade out -> intensity 0
        yield return FadeIntensity(onIntensity, offIntensity, fadeDuration);

        routine = null;
    }

    private IEnumerator FadeIntensity(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = duration <= 0f ? 1f : (t / duration);
            light2D.intensity = Mathf.Lerp(from, to, k);
            yield return null;
        }

        light2D.intensity = to;
    }
}
