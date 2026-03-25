using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 10f;
    public float gravity = -19.62f;
    public float jumpHeight = 3f;

    // --- SISTEM DETEKSI TANAH (BARU) ---
    public Transform groundCheck;
    public float groundDistance = 0.4f; // Jari-jari (radius) bola sensor
    public LayerMask groundMask;        // Filter komputasi fisika
    private bool isGrounded;
    // -----------------------------------

    private Vector3 velocity;

    void Update()
    {
        // 1. EVALUASI INTERSEKSI BOLA
        // Menghasilkan True jika bola imajiner di kaki pemain menyentuh objek ber-Layer 'Ground'
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            // Memaksa pemain tetap menempel di tanah untuk mencegah efek meluncur (sliding)
            velocity.y = -2f; 
        }

        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");   

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // 2. EKSEKUSI LOMPATAN
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}