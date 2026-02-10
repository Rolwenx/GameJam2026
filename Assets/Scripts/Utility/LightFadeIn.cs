using UnityEngine;

public class LightFadeIn : MonoBehaviour
{
    public float fadeSpeed = 2f;
    public float maxIntensity = 5f;

    private Light myLight;

    void Start()
    {
        myLight = GetComponent<Light>();
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