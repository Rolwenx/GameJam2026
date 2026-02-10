using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LightFadeIn : MonoBehaviour
{
    public float fadeSpeed = 2f;
    public float maxIntensity = 5f;

    private Light2D myLight;

    void Start()
    {
        myLight = GetComponent<Light2D>();
        myLight.intensity = 0f;
    }

    void Update()
    {
        if (myLight.intensity < maxIntensity)
        {
            myLight.intensity += fadeSpeed * Time.deltaTime;
        }
    }
}