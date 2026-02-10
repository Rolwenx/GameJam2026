using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

using System.Linq;
public class SwitchMenu : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuParametres;
    public GameObject menuElementsDébloqués;

    public TMP_Text ContinuerButton;


    void Start()
    {
        if(menuPrincipal.name == "MenuPause"){
                menuPrincipal.SetActive(false);
            }
            else {
                menuPrincipal.SetActive(true);
        }
        menuParametres.SetActive(false);
        if (menuElementsDébloqués != null)
            menuElementsDébloqués.SetActive(false);
        
        RefreshContinuerText();

    }
    
     public void GoToParam()
    {
        menuPrincipal.SetActive(false);
        menuParametres.SetActive(true);
    }

    public void GoToMenuPrincipal()
    {
        menuPrincipal.SetActive(true);
        menuParametres.SetActive(false);
        if (menuElementsDébloqués != null)
            menuElementsDébloqués.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit(); // marche uniquement dans une build, pas dans l'éditeur
    }

    public void GoToElementsDébloqués()
    {
        menuPrincipal.SetActive(false);
        menuElementsDébloqués.SetActive(true);
    }

    public void StartGame()
    {
        const string PREF_VOLUME = "MusicVolume"; // mets le même nom que dans GestionVolume

        // 1) garder le son
        float volume = PlayerPrefs.GetFloat(PREF_VOLUME, 1f);

        // 2) reset tout
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat(PREF_VOLUME, volume);
        InitializeNewGamePrefs();


        SceneManager.LoadScene("IntroCinematiqueDebut");
    }

    public void Continuer()
    {
        // Chercher la dernière scène jouée (en se basant sur les niveaux complétés)
        string current = SceneManager.GetActiveScene().name;
        
        string lastScene = PlayerPrefs.GetString("LastScene",SceneManager.GetActiveScene().name);

        if (lastScene == current || lastScene == "MainMenu")
        {
            if (ContinuerButton != null)
            return;
        }
        SceneManager.LoadScene(lastScene);
    }
    
    private void InitializeNewGamePrefs()
    {
        // audio
        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetFloat("MusicVolume", 1f);
        if (!PlayerPrefs.HasKey("SfxVolume"))

        // level progression
        for (int i = 1; i <= 4; i++)
        {
            PlayerPrefs.SetInt($"Level_{i}_Completed", 0);
            PlayerPrefs.SetInt($"LV{i}Valide", 0);
        }

        PlayerPrefs.SetInt("AllLevelsCompleted", 0);
        PlayerPrefs.SetInt("GameFinished", 0);

        // Lore
        for (int s = 1; s <= 4; s++)
        {
            for (int l = 1; l <= 2; l++)
            {
                PlayerPrefs.SetInt($"Lore_S{s}_L{l}", 0);
            }
        }

        PlayerPrefs.SetInt("AllLoreRead", 0);

        // scene, cinematique
        PlayerPrefs.SetString("LastScene", "IntroCinematiqueDebut");
        PlayerPrefs.SetInt("CityScene_Visited", 0);

        PlayerPrefs.Save();
    }

    void Update()
    {
        RefreshContinuerText();
    }


    private void RefreshContinuerText()
    {
        if (ContinuerButton == null) return;

        string current = SceneManager.GetActiveScene().name;
        string lastScene = PlayerPrefs.GetString("LastScene", current);

        ContinuerButton.text = (lastScene == current || lastScene == "MainMenu")
            ? "<sketchy>Aucun essai"
            : "<sketchy>Continuer";
    }



    

}
