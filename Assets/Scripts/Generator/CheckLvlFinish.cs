    using UnityEngine;
    using UnityEngine.Rendering.Universal;

    public class CheckLvlFinish : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer rune;
        public bool lvl1Finished;
        public bool lvl2Finished;
        public bool lvl3Finished;
        public bool lvl4Finished;

        public Light2D light2D;

        private void Awake()
        {
            if (rune == null) rune = GetComponent<SpriteRenderer>();
            if (light2D == null) light2D = GetComponentInChildren<Light2D>();
            light2D.intensity = 0f; // on commence avec la lumière éteinte

            SetColorWhenArrivingInScene();

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;

            if (gameObject.tag == "LV1" && lvl1Finished)
            {
                Debug.Log("Level 1 finished, changing rune color");
                rune.color = new Color32(0, 128, 251, 255);  
                // on fait la mm chose avec la lumière pour que ça fasse plus joli
                if (light2D != null) light2D.intensity = 0.7f;
                light2D.color = new Color32(0, 128, 251, 255);
                //player preference pour sauvegarder la progression du joueur, à utiliser pour le menu de sélection de niveau
                PlayerPrefs.SetInt("LV1Valide", 1);
                PlayerPrefs.Save();
                
            }
            else if (gameObject.tag == "LV2" && lvl2Finished)
            {
                Debug.Log("Level 2 finished, changing rune color");
                rune.color = new Color32(0, 255, 16, 255);
                if (light2D != null) light2D.intensity = 0.7f;
                light2D.color = new Color32(0, 255, 16, 255);
                PlayerPrefs.SetInt("LV2Valide", 1);
                PlayerPrefs.Save();
            }
            else if (gameObject.tag == "LV3" && lvl3Finished)
            {   
                rune.color = new Color32(255, 33, 0, 255);
                if (light2D != null) light2D.intensity = 0.7f;
                light2D.color = new Color32(255, 33, 0, 255);
                PlayerPrefs.SetInt("LV3Valide", 1);
                PlayerPrefs.Save();
            }
            else if (gameObject.tag == "LV4" && lvl4Finished)
            {
                rune.color = new Color32(255, 255, 255, 255);
                if (light2D != null) light2D.intensity = 0.7f;
                light2D.color = new Color32(255, 255, 255, 255);
                PlayerPrefs.SetInt("LV4Valide", 1);
                PlayerPrefs.Save();
            }
        }


         private void SetColorWhenArrivingInScene()
    {
        if (CompareTag("LV1") && PlayerPrefs.GetInt("LV1Valide", 0) == 1)
        {
            lvl1Finished = true;
            Apply(new Color32(0, 128, 251, 255));
        }
        else if (CompareTag("LV2") && PlayerPrefs.GetInt("LV2Valide", 0) == 1)
        {
            lvl2Finished = true;
            Apply(new Color32(0, 255, 16, 255));
        }
        else if (CompareTag("LV3") && PlayerPrefs.GetInt("LV3Valide", 0) == 1)
        {
            lvl3Finished = true;
            Apply(new Color32(255, 33, 0, 255));
        }
        else if (CompareTag("LV4") && PlayerPrefs.GetInt("LV4Valide", 0) == 1)
        {
            lvl4Finished = true;
            Apply(new Color32(255, 255, 255, 255));
        }
    }

    private void Apply(Color32 c)
    {
        if (rune != null) rune.color = c;

        if (light2D != null)
        {
            light2D.color = c;
            light2D.intensity = 0.7f;
        }
    }
}
    
