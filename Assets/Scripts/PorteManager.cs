using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PorteManager : MonoBehaviour
{
    private OuverturePorte lastDoor;
    private bool finalTriggered = false;

    [Header("Light Settings")]
    [SerializeField] private Light2D playerLight;

    private void Start()
    {
        int lastIndex = transform.childCount - 1;

        lastDoor = transform.GetChild(lastIndex)
                            .GetComponent<OuverturePorte>();

        if(lastDoor != null)
            lastDoor.OnDoorOpened += HandleFinalDoor;
        else
            Debug.LogWarning("Last door missing OuverturePorte.");
    }

    private void HandleFinalDoor()
    {
        if(finalTriggered) return;

        finalTriggered = true;


        StartCoroutine(ExpandLight());
    }

    private IEnumerator ExpandLight()
    {
        float start = playerLight.pointLightOuterRadius;
        float target = 500f;
        float duration = 15f;

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float k = Mathf.SmoothStep(0, 1, t / duration);

            playerLight.pointLightOuterRadius =
                Mathf.Lerp(start, target, k);

            yield return null;
        }

        playerLight.pointLightOuterRadius = target;
    }

    private void OnDestroy()
    {
        if(lastDoor != null)
            lastDoor.OnDoorOpened -= HandleFinalDoor;
    }
}