using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;



public class DefiningLore : MonoBehaviour
{
    public TMP_Text loreText;

    private string[][] allScenesLore;

    private string[] Scene1Lore = { "Verrélume n’a jamais eu peur du noir… jusqu’au jour où la lumière s’est tue.", "Le Cœur-Lux transformait les gros éclats en jour artificiel. On l’appelait ‘le Soleil du village’." };
    private string[] Scene2Lore = { "Quand la lumière faiblissait, on descendait chercher d’autres éclats… toujours plus profond.", "Les anciens gravaient une règle : ‘Ne dirige jamais le faisceau vers le Cœur." };
    private string[] Scene3Lore = { "La nuit du Chaos, quelqu’un a retiré les éclats du générateur. La ville s’est éteinte en une minute.", "Après ça, ils ont muré des couloirs. Comme si quelque chose voulait remonter." };
    private string[] Scene4Lore = { "Ils n’ont pas caché les éclats pour les garder… ils les ont cachés pour empêcher le Cœur de se réveiller." ,"Le Cœur-Lux ne se nourrit pas de fragments. Il se nourrit d’une source vivante. Et elle est déjà là."};
    [SerializeField] private float fadeOutDuration = 0.25f;
    private Coroutine fadeOutCo;

    public AudioSource audioSource;

    private void Awake()
    {
        allScenesLore = new string[][]
        {
            Scene1Lore,
            Scene2Lore,
            Scene3Lore,
            Scene4Lore
        };

        // Initialise un PlayerPref pour chaque lore (0 = pas lu)
        for (int s = 0; s < allScenesLore.Length; s++)
        {
            for (int l = 0; l < allScenesLore[s].Length; l++)
            {
                int sceneNumber = s + 1; // Scene1 -> 1
                int loreNumber  = l + 1; // Lore1  -> 1

                string key = $"Lore_S{sceneNumber}_L{loreNumber}";

                // si la clé n'existe pas, on la crée à 0
                if (!PlayerPrefs.HasKey(key))
                    PlayerPrefs.SetInt(key, 0);
            }
        }

        PlayerPrefs.Save();
    }

    void Start()
    {
        loreText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.Play();
        if (!collision.gameObject.CompareTag("Player")) return;

        string currentSceneName = SceneManager.GetActiveScene().name;

        // dernier caractère doit être un chiffre (ex: "Scene1")
        char lastChar = currentSceneName[currentSceneName.Length - 1];
        if (!char.IsDigit(lastChar)) return;

        int sceneIndex = (lastChar - '0') - 1; // Scene1 -> 0

        // dernier caractère du nom de l'objet doit être un chiffre (ex: "Lore2")
        char objChar = gameObject.name[gameObject.name.Length - 1];
        if (!char.IsDigit(objChar)) return;

        int loreIndex = (objChar - '0') - 1; // Lore1 -> 0

        if (sceneIndex < 0 || sceneIndex >= allScenesLore.Length) return;
        if (loreIndex < 0 || loreIndex >= allScenesLore[sceneIndex].Length) return;

        loreText.gameObject.SetActive(true);
        loreText.text = "<wave uniformity=0.02>" + allScenesLore[sceneIndex][loreIndex];
        // attendre 5 sec avant d'effacer le texte
        Invoke(nameof(ClearLoreText), 8f);

        // Marquer ce lore comme lu (1)
        string key = $"Lore_S{sceneIndex + 1}_L{loreIndex + 1}";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        Debug.Log($"Player read {key}");
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // cacher l'objet après lecture
        gameObject.GetComponent<Collider2D>().enabled = false; // désactiver le trigger pour éviter les relectures



        // s'ils sont tous activés, débloquer le menu de lore dans le menu principal
        bool allLoreRead = true;
        for (int s = 0; s < allScenesLore.Length; s++)
        {
            for (int l = 0; l < allScenesLore[s].Length; l++)
            {
                string k = $"Lore_S{s + 1}_L{l + 1}";
                if (PlayerPrefs.GetInt(k, 0) == 0)
                {
                    allLoreRead = false;
                    break;
                }
            }
            if (!allLoreRead) break;
        }

        if (allLoreRead)
        {
            PlayerPrefs.SetInt("AllLoreRead", 1);
            PlayerPrefs.Save();
        }


        //afficher un message de déblocage du menu de lore si c'est la dernière info à lire
        if (allLoreRead)
        {
            // ✅ AJOUT: afficher un message de déblocage
            loreText.text = "<wave uniformity=0.02>Tu as découvert tous les éléments de l'histoire ! Tu peux maintenant accéder au dernier secret caché dans le menu principal.";
            // attendre 5 sec avant d'effacer le texte
            Invoke(nameof(ClearLoreText), 8f);
        }
    }

     void ClearLoreText()
    {   
        // ✅ AJOUT: fade out au lieu de vider direct
        if (fadeOutCo != null) StopCoroutine(fadeOutCo);
        fadeOutCo = StartCoroutine(FadeOutThenClear());
    }

    // ✅ AJOUT: coroutine fade out
    private IEnumerator FadeOutThenClear()
    {
        Color c = loreText.color;
        float startA = c.a;

        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeOutDuration);
            c.a = Mathf.Lerp(startA, 0f, k);
            loreText.color = c;
            yield return null;
        }

        c.a = 0f;
        loreText.color = c;

        loreText.text = "";
        c.a = 1f;
        loreText.color = c;
        fadeOutCo = null;
        loreText.gameObject.SetActive(false);
    }
}
