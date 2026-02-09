using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ReloadSceneGeneral : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
