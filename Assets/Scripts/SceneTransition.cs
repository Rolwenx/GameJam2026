using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void GoToTown()
    {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Town");    
    }
     public void GoToSceneChooser()
    {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Pick Level");    
    }

    public void GoToGenerator()
    {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Generator");    
    }
}
