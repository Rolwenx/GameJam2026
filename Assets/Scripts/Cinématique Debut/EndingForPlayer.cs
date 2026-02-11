using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EndingForPlayer : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer playerSprite;
    public Light2D playerLight;

    [Header("Light Settings")]
    public float fadeSpeed = 2f;
    public float lightTargetRadius = 50f;

    [Header("Ending")]
    public string nextSceneName = "Credit";

    private bool fadingOut = false;
    private bool fadingIn = false;
    private bool endingFade = false;

    void Update()
    {
        // ───── Fade OUT (normal) ─────
        if (fadingOut)
        {
            playerLight.pointLightOuterRadius -= fadeSpeed * Time.deltaTime;

            if (playerLight.pointLightOuterRadius <= 0f)
            {
                playerLight.pointLightOuterRadius = 0f;
                fadingOut = false;
            }
        }

        // ───── Fade IN ─────
        if (fadingIn)
        {
            playerLight.pointLightOuterRadius += fadeSpeed * Time.deltaTime;

            if (playerLight.pointLightOuterRadius >= lightTargetRadius)
            {
                playerLight.pointLightOuterRadius = lightTargetRadius;
                fadingIn = false;
            }
        }

        // ───── FINAL FADE + SCENE CHANGE ─────
        if (endingFade)
        {
            playerLight.pointLightOuterRadius -= fadeSpeed * Time.deltaTime;

            if (playerLight.pointLightOuterRadius <= 0f)
            {
                playerLight.pointLightOuterRadius = 0f;
                PlayerPrefs.SetInt("GameFinished", 1);
                PlayerPrefs.Save();
                GameStateManager.Instance.RefreshState();
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
    public void Disappear()
    {
        playerSprite.enabled = false;
        fadingOut = true;
        fadingIn = false;
    }

    public void LightBack()
    {
        fadingIn = true;
        fadingOut = false;
    }

    public void EndGameWithLightFade()
    {
        Debug.Log("END GAME FADE STARTED");
        Time.timeScale = 1f; 
        if (playerLight.pointLightOuterRadius <= 0.1f)
            playerLight.pointLightOuterRadius = 0.1f;
        endingFade = true;
        fadingOut = false;
        fadingIn = false;
    }
}