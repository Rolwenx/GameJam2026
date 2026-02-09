using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering.Universal;

public class LightCrystal : MonoBehaviour
{
    [Header("Light Settings")]
    private float onIntensity = 0.7f;
    [SerializeField] private float offIntensity = 0f;

    [Header("Timings")]
    [SerializeField] private float fadeDuration = 0.12f;
    [SerializeField] private float stayLitTime = 3f;
    [SerializeField] private float delayBeforeExplode = 1.5f; // 1–2 sec

    [Header("Fall")]
    [SerializeField] private float fallGravityScale = 1f;
    private Collider2D crystalCollider;

    private Light2D light2D;
    private Coroutine routine, easy_routine;

    private GameObject[] cristalChildren;
    [Header("Tutorial")]
    [SerializeField] public bool isItTutorial = true;
    public bool hasTutorialbeenDone = false;
    public static event Action OnTutorialCrystalFinished;
    public static event Action AfterEnchainementCristal;

    private bool lockedOn = false; // pour éviter que le cristal soit réactivé après avoir été désactivé par la porte

    private GameObject porte;


    private void Awake()
    {
        cristalChildren = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cristalChildren[i] = transform.GetChild(i).gameObject;
        }

        light2D = cristalChildren[0].GetComponent<Light2D>();
        light2D.intensity = offIntensity;

        // prend le rigibody et le met en kinematic dès le départ pour éviter que le cristal tombe avant d'être activé
        if (gameObject.GetComponent<Rigidbody2D>() == null) gameObject.AddComponent<Rigidbody2D>();
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

    }

    public void Calltolight(bool petitCristal)
    {

        if (lockedOn) return;
        gameObject.tag = "Lit";
        if(petitCristal){
      
            if (crystalCollider == null) crystalCollider = GetComponent<Collider2D>();
                if (light2D == null) return;

            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(SequenceFalling());
        }
        else if (isItTutorial == true)
        {
            if (routine != null) StopCoroutine(routine);
            routine = StartCoroutine(SequenceEasyLevel());
        }
        else {
            routine = StartCoroutine(Sequence());
            gameObject.tag = "Unlit";
        }

    }

    private IEnumerator SequenceEasyLevel()
    {
        yield return FadeIntensity(offIntensity, onIntensity, fadeDuration);
        porte = GameObject.Find("PorteInvisbleContainer"); 
        yield return new WaitForSeconds(stayLitTime);
        yield return FadeIntensity(onIntensity, offIntensity, fadeDuration);
           
        OuverturePorte OuvertePorte = porte.GetComponent<OuverturePorte>();

        if (isItTutorial && !hasTutorialbeenDone)
        {
            hasTutorialbeenDone = true;

            OnTutorialCrystalFinished?.Invoke();
        }
        routine = null;
        gameObject.tag = "Unlit";
    }

    private IEnumerator SequenceFalling()
    {
        // 1) allume
        yield return FadeIntensity(offIntensity, onIntensity, fadeDuration);

        // 2) reste allumé
        yield return new WaitForSeconds(stayLitTime);

        // 3) éteint
        yield return FadeIntensity(onIntensity, offIntensity, fadeDuration);

    
        StartFalling();

        routine = null;
    }

        private IEnumerator Sequence()
    {
        // 1) allume
        yield return FadeIntensity(offIntensity, onIntensity, fadeDuration);

        // 2) reste allumé
        yield return new WaitForSeconds(stayLitTime);

        // 3) éteint
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


    private void StartFalling()
    {
        if (crystalCollider != null)
            crystalCollider.enabled = false;

        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
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

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;    
        bc.isTrigger = false; // s'assure que le cristal peut interagir avec le sol après être tombé

    }

    public void CancelAll()
    {
        lockedOn = true;
        StopAllCoroutines();
        if (light2D != null) light2D.intensity = onIntensity; 

    }

}
