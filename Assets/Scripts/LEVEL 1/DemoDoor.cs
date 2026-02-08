using UnityEngine;

public class DemoDoor : MonoBehaviour
{
    public bool hasCollided = false;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            hasCollided = true;

        }
    }
}
