using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    // Destroy any object on collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
    }
}
