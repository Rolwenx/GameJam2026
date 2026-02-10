using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class ReloadSceneGeneral : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // reload scene actuelle
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
