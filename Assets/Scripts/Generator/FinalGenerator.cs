using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cainos.PixelArtTopDown_Basic
{
    public class FinalGenerator : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed = 6f;

        [Header("End UI")]
        public GameObject endCanvas;
        public string endSceneName = "Ending";

        private bool isInside = false;
        private bool allValid = false;
        private bool canvasShown = false;
        // on garde une couleur courante par rune (sinon elles prennent toutes la même)
        private Color[] curColors;
        private Color[] baseColors;


        private void Awake()
        {


            int n = runes.Count;
            curColors = new Color[n];
            baseColors = new Color[n];

            for (int i = 0; i < n; i++)
            {
                // couleur "de base" = couleur du sprite dans l'inspector, mais alpha=1
                baseColors[i] = runes[i].color;
                baseColors[i].a = 1f;

                // départ: même couleur mais alpha 0 (éteint)
                curColors[i] = baseColors[i];
                curColors[i].a = 0f;

                runes[i].color = curColors[i];
            }

            if (endCanvas != null)
                endCanvas.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            isInside = true;
            CheckAllLevels();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            isInside = false;
            HideCanvas();
        }

        private void Update()
        {
            
            float targetAlpha = (isInside && allValid) ? 1f : 0f;

            for (int i = 0; i < runes.Count; i++)
            {
                Color target = baseColors[i];
                target.a = targetAlpha;

                curColors[i] = Color.Lerp(curColors[i], target, lerpSpeed * Time.deltaTime);
                runes[i].color = curColors[i];
            }

            if (isInside && allValid && canvasShown)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SceneManager.LoadScene(endSceneName);
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    HideCanvas();
                }
            }
        }

        private void CheckAllLevels()
        {
            allValid =
                PlayerPrefs.GetInt("LV1Valide", 0) == 1 &&
                PlayerPrefs.GetInt("LV2Valide", 0) == 1 &&
                PlayerPrefs.GetInt("LV3Valide", 0) == 1 &&
                PlayerPrefs.GetInt("LV4Valide", 0) == 1;

            PlayerPrefs.SetInt("AllLevelsCompleted", allValid ? 1 : 0);
            PlayerPrefs.Save();

            if (allValid)
                ShowCanvas();
        }
        

        private void ShowCanvas()
        {
            if (endCanvas == null || canvasShown) return;

            endCanvas.SetActive(true);
            canvasShown = true;
        }

        private void HideCanvas()
        {
            if (endCanvas == null) return;

            endCanvas.SetActive(false);
            canvasShown = false;
        }



    }
}
