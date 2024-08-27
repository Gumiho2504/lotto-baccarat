using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        // Get the Rigidbody2D component attached to the ball
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the ball collided with an object tagged "Bar"
        if (collision.gameObject.CompareTag("Bar"))
        {
           // AudioController.Instance.PlaySFX("ball");
            // Get the direction of the push from the bar
            Vector2 pushDirection = collision.contacts[0].normal;

            // Apply force to the ball in the direction away from the bar
            float pushForce = 1f; // Adjust this value as needed
            rb.AddForce(-pushDirection * pushForce, ForceMode2D.Impulse);
        }
      else if (  collision.gameObject.CompareTag("Bar"))
        {
            Vector2 pushDirection = collision.contacts[0].normal;
            float pushForce = 0f; // Adjust this value as needed
            rb.AddForce(-pushDirection * pushForce, ForceMode2D.Impulse);
        }
    }
}
