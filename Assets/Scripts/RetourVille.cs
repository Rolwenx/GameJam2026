using UnityEngine;

public class RetourVille : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //load la scene avec le nom TOwn
            UnityEngine.SceneManagement.SceneManager.LoadScene("Town"); 
        }
    }
}
