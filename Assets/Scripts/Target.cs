using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Melaporkan kematian ke GameManager sentral sebelum hancur
        if (GameManager.instance != null)
        {
            GameManager.instance.TargetDestroyed();
        }

        Destroy(gameObject);
    }
}