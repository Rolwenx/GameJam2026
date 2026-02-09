using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private bool firstimeHitGround;

    private Vector3 initialPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firstimeHitGround = true;
    }


    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (firstimeHitGround) {
            initialPosition = transform.position;
            firstimeHitGround = false;
        }
        else {
            if (collision.gameObject.name.Contains("Obstacles") || collision.gameObject.name.Contains("LimitMap") || collision.gameObject.name.Contains("Danger")) {
                transform.position = initialPosition;
                Debug.Log("Reset position to: " + initialPosition);
            }
        }
    }



}