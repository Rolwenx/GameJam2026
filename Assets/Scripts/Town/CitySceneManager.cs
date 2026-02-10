using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class CitySceneManager : MonoBehaviour
{
    private const string CITY_VISITED_KEY = "CityScene_Visited";
    private const string LAST_SCENE_KEY = "LastScene";

    [Header("Player Spawn Points")]
    [SerializeField] private Transform defaultSpawnPoint;
    [SerializeField] private Transform generatorSpawnPoint;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform player;
    [SerializeField] private Transform generatorCamera;
    [SerializeField] private Light2D generatorLight;
    [SerializeField] private GameObject level_choice;
    [SerializeField] private Cainos.PixelArtTopDown_Basic.CameraFollow cameraFollow;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text introText;

    [Header("Timings")]
    [SerializeField] private float textDisplayTime = 3f;
    [SerializeField] private float cameraMoveDuration = 1.5f;

    private Vector3 cameraStartPos;

    void Start()
    {
        SetupPlayerSpawn();
        panel.SetActive(false);
        level_choice.SetActive(false);

        if (PlayerPrefs.GetInt(CITY_VISITED_KEY, 0) == 0)
        {
            Debug.Log("City intro started");
            StartCoroutine(FirstTimeCityIntro());
            
        }
        else
        {
            level_choice.SetActive(true);
        }
    }

    private void SetupPlayerSpawn()
    {
        string lastScene = PlayerPrefs.GetString(LAST_SCENE_KEY, "");

        if (lastScene == "Generator" && generatorSpawnPoint != null)
        {
            player.position = generatorSpawnPoint.position;
        }
        else if (defaultSpawnPoint != null)
        {
            player.position = defaultSpawnPoint.position;
        }
    }

    private IEnumerator FirstTimeCityIntro()
    {
        
        PlayerPrefs.SetInt(CITY_VISITED_KEY, 1);
        PlayerPrefs.Save();
        

        Time.timeScale = 0f;
        if (cameraFollow != null)
            cameraFollow.enabled = false;

        yield return new WaitForSecondsRealtime(2f);

        cameraStartPos = mainCamera.transform.position;
        panel.SetActive(true);

        introText.text =
        "Voici le village.\n\n" +
        "Il est plongé dans le noir depuis bien trop d'années.\n\n" +
        "Il s’illuminera peu à peu à mesure que vous débloquerez des niveaux.\n\n" +
        "Explorez-le et parlez aux habitants.\n" +
        "Ils pourraient dire des choses différentes au fil du temps.\n\n" +
        "<size=70%><i>(Clique gauche pour continuer)</i></size>";

        yield return WaitForLeftClick();

        introText.text =
        "Si vous cherchez votre progression...\n" +
        "Voici la zone du générateur de lumière.\n\n" +
        "<size=70%><i>(Clique gauche)</i></size>";

        yield return WaitForLeftClick();

        yield return MoveCamera(generatorCamera.position);
        // --- RETOUR JOUEUR ---
        introText.text =
        "C’est ici que vous pourrez observer l’avancée de la lumière.\n\n" +
        "<size=70%><i>(Clique gauche pour revenir)</i></size>";

        yield return WaitForLeftClick();

        yield return MoveCamera(cameraStartPos);
        generatorLight.intensity = 0;

        panel.SetActive(false);

        if (cameraFollow != null)
        {
            cameraFollow.RecalculateOffset();
            cameraFollow.enabled = true;
        }
        Time.timeScale = 1f;
        level_choice.SetActive(true);
        
    }

    private IEnumerator WaitForLeftClick()
    {
        // On attend que le joueur relâche le clic (au cas où)
        yield return new WaitUntil(() => !Input.GetMouseButton(0));

        // Puis on attend un clic gauche
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }

    private IEnumerator MoveCamera(Vector3 targetPos)
    {
        Vector3 startPos = mainCamera.transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / cameraMoveDuration;
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }
}