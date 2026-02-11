using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SeeLoreMainMenu : MonoBehaviour
{
        private string[] Scene1Lore = { "Verrélume brillait même sans soleil… jusqu’au jour où tout s’est arrêté.", "Au centre du village, le Cœur-Lux transformait les grands éclats en jour artificiel. On l’appelait le « Soleil du village »" };
    private string[] Scene2Lore =
    {
        "Quand la lumière faiblissait, on descendait chercher d’autres éclats… toujours plus profond sous la ville.",
        "Les anciens avaient gravé une règle : « Ne dirige jamais le faisceau vers le Cœur. »"
    };

        private string[] Scene3Lore =
    {
        "La Nuit du Chaos, quelqu’un a retiré les éclats du générateur. En une minute, le village s’est éteint.",
        "Après ça, des couloirs ont été murés… comme si quelque chose, en bas, devait rester enfermé."
    };

    private string[] Scene4Lore =
    {
        "Ils n’ont pas caché les éclats pour les garder… mais pour empêcher le Cœur de se réveiller.",
        "Le Cœur-Lux n’attend pas seulement de la lumière. Il attend une source… vivante."
    };
    
    public TMP_Text TitleScene;
    public TMP_Text LoreText1;
    public TMP_Text LoreText2;
    public GameObject LoreCanvas;
    public GameObject MenuLoreCanvas;

    public Button SecretButton;

    public GameObject Secret;

     void Start()
    {
        LoreCanvas.SetActive(false);
        Secret.SetActive(false);
    }
        

    public void Scene1Button()
    {
        LoreCanvas.SetActive(true);
        MenuLoreCanvas.SetActive(false);
        TitleScene.text = "<sketchy>Eléments level 1";
        if (PlayerPrefs.GetInt("Lore_S1_L1") == 1)
            LoreText1.text = Scene1Lore[0];
        else
            LoreText1.text = "???";
        
        if (PlayerPrefs.GetInt("Lore_S1_L2") == 1)
            LoreText2.text = Scene1Lore[1];
        else
            LoreText2.text = "???";

    }

    public void Scene2Button()
    {
        LoreCanvas.SetActive(true);
            MenuLoreCanvas.SetActive(false);
        TitleScene.text = "<sketchy>Eléments level 2";
        if (PlayerPrefs.GetInt("Lore_S2_L1") == 1)
            LoreText1.text = Scene2Lore[0];
        else
            LoreText1.text = "???";
        
        if (PlayerPrefs.GetInt("Lore_S2_L2") == 1)
            LoreText2.text = Scene2Lore[1];
        else
            LoreText2.text = "???";
    }

    public void Scene3Button()
    {
        LoreCanvas.SetActive(true);
            MenuLoreCanvas.SetActive(false);
        TitleScene.text = "<sketchy>Eléments level 3";
        if (PlayerPrefs.GetInt("Lore_S3_L1") == 1)
            LoreText1.text = Scene3Lore[0];
        else
            LoreText1.text = "???";
        
        if (PlayerPrefs.GetInt("Lore_S3_L2") == 1)
            LoreText2.text = Scene3Lore[1];
        else
            LoreText2.text = "???";
    }

    public void Scene4Button()
    {
        LoreCanvas.SetActive(true);
            MenuLoreCanvas.SetActive(false);
        TitleScene.text = "<sketchy>Eléments level 4";
        if (PlayerPrefs.GetInt("Lore_S4_L1") == 1)
            LoreText1.text = Scene4Lore[0];
        else
            LoreText1.text = "???";
        
        if (PlayerPrefs.GetInt("Lore_S4_L2") == 1)
            LoreText2.text = Scene4Lore[1];
        else
            LoreText2.text = "???";
    }

    public void BackToMenu()
    {
        LoreCanvas.SetActive(false);
        MenuLoreCanvas.SetActive(true);
        Secret.SetActive(false);
    }

    public void SecretButtonClicked()
    {
        Secret.SetActive(true);
        MenuLoreCanvas.SetActive(false);
        LoreCanvas.SetActive(false);
    }

    void Update(){
        if (PlayerPrefs.GetInt("AllLoreRead", 0) == 1)
        {
            SecretButton.GetComponent<Button>().interactable = true;
            SecretButton.GetComponent<UnityEngine.UI.Image>().color = new Color32(46, 171, 86, 255);
        }
        else {
            
            SecretButton.GetComponent<UnityEngine.UI.Image>().color = new Color32(20, 63, 34, 255);
            SecretButton.GetComponent<Button>().interactable = false;
        }
    }

    
}
