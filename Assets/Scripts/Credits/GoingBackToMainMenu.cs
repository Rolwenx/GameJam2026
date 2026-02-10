using UnityEngine;

public class GoingBackToMainMenu : MonoBehaviour
{
    void Start(){
        // attendre 15 sec
        Invoke("LoadMainMenu", 15f);
    }

    public void LoadMainMenu(){
        // charger la scène du menu principal
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


}
