using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class FinalGenerator : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed = 6f;

        private bool isInside = false;

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
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            isInside = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            isInside = false;
        }

        private void Update()
        {
            bool allValid =
                PlayerPrefs.GetInt("LV1Valide", 0) == 1 &&
                PlayerPrefs.GetInt("LV2Valide", 0) == 1 &&
                PlayerPrefs.GetInt("LV3Valide", 0) == 1 &&
                PlayerPrefs.GetInt("LV4Valide", 0) == 1;

            float targetAlpha = (isInside && allValid) ? 1f : 0f;

            for (int i = 0; i < runes.Count; i++)
            {
                Color target = baseColors[i];
                target.a = targetAlpha;

                curColors[i] = Color.Lerp(curColors[i], target, lerpSpeed * Time.deltaTime);
                runes[i].color = curColors[i];
            }
        }
    }
}
