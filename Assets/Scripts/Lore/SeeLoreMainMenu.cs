using UnityEngine;
using TMPro;

public class SeeLoreMainMenu : MonoBehaviour
{
    private string[] Scene1Lore = { "Verrélume n’a jamais eu peur du noir… jusqu’au jour où la lumière s’est tue.", "Le Cœur-Lux transformait les gros éclats en jour artificiel. On l’appelait ‘le Soleil du village’." };
    private string[] Scene2Lore = { "Quand la lumière faiblissait, on descendait chercher d’autres éclats… toujours plus profond.", "Les anciens gravaient une règle : ‘Ne dirige jamais le faisceau vers le Cœur." };
    private string[] Scene3Lore = { "La nuit du Chaos, quelqu’un a retiré les éclats du générateur. La ville s’est éteinte en une minute.", "Après ça, ils ont muré des couloirs. Comme si quelque chose voulait remonter." };
    private string[] Scene4Lore = { "Ils n’ont pas caché les éclats pour les garder… ils les ont cachés pour empêcher le Cœur de se réveiller." ,"Le Cœur-Lux ne se nourrit pas de fragments. Il se nourrit d’une source vivante. Et elle est déjà là."};
    
    
    public TMP_Text TitleScene;
    public TMP_Text LoreText1;
    public TMP_Text LoreText2;
    public GameObject LoreCanvas;
    public GameObject MenuLoreCanvas;

     void Start()
    {
        LoreCanvas.SetActive(false);
    }
        

    public void Scene1Button()
    {
        LoreCanvas.SetActive(true);
        MenuLoreCanvas.SetActive(false);
        TitleScene.text = "Eléments level 1";
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
        TitleScene.text = "Eléments level 2";
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
        TitleScene.text = "Eléments level 3"    ;
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
        TitleScene.text = "Eléments level 4"    ;
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
    }

    
}
