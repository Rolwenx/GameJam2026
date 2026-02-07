using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class LightCrystal : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private float onIntensity = 2f;
    [SerializeField] private float offIntensity = 0f;

    [Header("Timings")]
    [SerializeField] private float fadeDuration = 0.12f;
    [SerializeField] private float stayLitTime = 3f;
    [SerializeField] private float delayBeforeExplode = 1.5f; // 1–2 sec

    [Header("Explosion")]
    [SerializeField] private string explodeTriggerName = "Explode";

    [Header("Fall")]
    [SerializeField] private float fallGravityScale = 1f;
    [SerializeField] private Collider2D crystalCollider;

    private Light2D light2D;
    private Animator anim;
    private GameObject explosionEffect;
    private Coroutine routine;

    private void Awake()
    {
        light2D = transform.GetChild(0).GetComponent<Light2D>();
        explosionEffect = gameObject.transform.GetChild(1).gameObject;
        anim = explosionEffect.GetComponent<Animator>();
        explosionEffect.SetActive(false); // désactive l'anim au départ pour économiser les ressources (il n'est pas visible tant que le cristal est éteint)

        if (light2D == null) Debug.LogError("LightCrystal: Light2D manquant sur l'enfant 0.");
        if (anim == null) Debug.LogError("LightCrystal: Animator manquant sur l'enfant 1.");

        if (light2D != null) light2D.intensity = offIntensity;

        if (crystalCollider == null) crystalCollider = GetComponent<Collider2D>();
    }

    public void Calltolight(bool hardLevel)
    {
        gameObject.tag = "Lit";
        Debug.Log("LightCrystal: Calltolight() appelé. HardLevel = " + hardLevel);  
        if(hardLevel){
            if (light2D == null || anim == null) return;

            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(Sequence());
        }

    }

    private IEnumerator Sequence()
    {
        // 1) allume
        yield return FadeIntensity(offIntensity, onIntensity, fadeDuration);

        // 2) reste allumé
        yield return new WaitForSeconds(stayLitTime);

        // 3) éteint
        yield return FadeIntensity(onIntensity, offIntensity, fadeDuration);

        // 4) délai avant explosion
        yield return new WaitForSeconds(delayBeforeExplode);
        explosionEffect.SetActive(true); // au cas où l'anim est désactivée pour économiser les ressources pendant qu'elle est éteinte
        // 5) explosion
        anim.ResetTrigger(explodeTriggerName);
        anim.SetTrigger(explodeTriggerName);

        // attendre 0,04 secondes (durée de l'explosion) avant de passer à la suite
        yield return new WaitForSeconds(0.4f);

        // 7) après l'explosion -> il tombe
        explosionEffect.SetActive(false); 
        StartFalling();

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

    private void StartFalling()
    {
        if (crystalCollider != null)
            crystalCollider.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        if (bc == null) bc = gameObject.AddComponent<BoxCollider2D>();

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravityScale;
        rb.freezeRotation = true;
        // exclure le cristal des collision du layer CameraBound

        int cameraBoundLayer = LayerMask.NameToLayer("CameraBound");
        Collider2D[] bounds = FindObjectsOfType<Collider2D>();

        foreach (var col in bounds)
        {
            if (col.gameObject.layer == cameraBoundLayer)
            {
                Physics2D.IgnoreCollision(bc, col, true);
            }
        }


        bc.isTrigger = false; // s'assure que le cristal peut interagir avec le sol après être tombé

    }
}
