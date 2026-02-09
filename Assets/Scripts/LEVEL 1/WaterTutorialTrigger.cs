using UnityEngine;

public class WaterTutorialTrigger : MonoBehaviour
{
    public bool hasCollidedWater = false;
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            hasCollidedWater = true;

        }
    }
}