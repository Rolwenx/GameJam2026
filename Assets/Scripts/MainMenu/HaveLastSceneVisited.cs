using UnityEngine;
using UnityEngine.SceneManagement;
public class HaveLastSceneVisited : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
        Debug.Log("Last scene saved: " + SceneManager.GetActiveScene().name);
    }


}
