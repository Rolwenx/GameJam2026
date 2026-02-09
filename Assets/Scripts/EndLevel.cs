using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndLevel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI endText;

    [Header("Scene")]
    [SerializeField] private string nextSceneName;

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float waitAfterFade = 2f;

    private void Awake()
    {
        panel.SetActive(false);

        Color c = endText.color;
        c.a = 0f;
        endText.color = c;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        StartCoroutine(EndSequence());
    }

    private IEnumerator EndSequence()
    {
        Time.timeScale = 0f;

        panel.SetActive(true);

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;

            float k = t / fadeDuration;

            Color c = endText.color;
            c.a = Mathf.Lerp(0, 1, k);
            endText.color = c;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(waitAfterFade);

        Time.timeScale = 1f;

        SceneManager.LoadScene(nextSceneName);
    }
}