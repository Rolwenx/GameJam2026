using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void GoToTown()
    {
        SceneManager.LoadScene("Town");    
    }
     public void GoToSceneChooser()
    {
        SceneManager.LoadScene("Pick Level");    
    }

    public void GoToGenerator()
    {
        SceneManager.LoadScene("Generator");    
    }
}
