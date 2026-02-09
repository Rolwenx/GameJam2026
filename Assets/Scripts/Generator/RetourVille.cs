using UnityEngine;
using UnityEngine.SceneManagement;

public class RetourVille : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //load la scene avec le nom TOwn
            PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Town"); 
        }
    }
}
