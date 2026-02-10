using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class SwitchMenu : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuParametres;
    public GameObject menuElementsDébloqués;


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
        SceneManager.LoadScene("IntroCinematiqueDebut");
    }


    




    

}
