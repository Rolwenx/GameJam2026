using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchToParam : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuParametres;
    public GameObject menuElementsDébloqués;


    void Start()
    {
        menuPrincipal.SetActive(true);
        menuParametres.SetActive(false);
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


    




    

}
