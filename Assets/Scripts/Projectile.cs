using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // MENGGANTI FUNGSI TRIGGER MENJADI COLLISION FISIK
    void OnCollisionEnter(Collision collision)
    {
        // Mengambil referensi skrip Target dari objek fisik yang tertabrak
        Target target = collision.gameObject.GetComponent<Target>();
        
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // Hancurkan peluru ini secara instan sesaat setelah membentur apapun
        Destroy(gameObject);
    }
}