using UnityEngine;

public class SwitchToParam : MonoBehaviour
{
    public GameObject menuPrincipal;
    public GameObject menuParametres;
    void Start()
    {
        menuPrincipal.SetActive(true);
        menuParametres.SetActive(false);
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
    }

    public void QuitGame()
    {
        Application.Quit(); // marche uniquement dans une build, pas dans l'éditeur
    }
}
