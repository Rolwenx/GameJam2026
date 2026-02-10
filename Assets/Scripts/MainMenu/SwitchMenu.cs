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
        PlayerPrefs.DeleteAll(); // pour reset les données de progression (tuto + lore)
        PlayerPrefs.Save();
        SceneManager.LoadScene("IntroCinematiqueDebut");
    }

    public void Continuer()
    {
        // Chercher la dernière scène jouée (en se basant sur les niveaux complétés)
        
        string lastScene = PlayerPrefs.GetString("LastScene",SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(lastScene);
    }
    




    

}
