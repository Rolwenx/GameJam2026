using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalTownLight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D playerLight;

    private void Start()
    {
        ApplyLightProgression();
    }

    private void ApplyLightProgression()
    {
        float radius = playerLight.pointLightOuterRadius;

        if (PlayerPrefs.GetInt("Level_1_Completed", 0) == 1)
            radius = 10f;
        else if (PlayerPrefs.GetInt("Level_2_Completed", 0) == 1)
            radius = 15f;
        else if (PlayerPrefs.GetInt("Level_3_Completed", 0) == 1)
            radius = 20f;
        else if (PlayerPrefs.GetInt("Level_4_Completed", 0) == 1)
            radius = 25f;
        else if (PlayerPrefs.GetInt("GameFinished", 0) == 1)
            radius = 50f;
        else
            radius = 5f;

        playerLight.pointLightOuterRadius = radius;
    }
}