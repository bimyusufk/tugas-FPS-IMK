using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Sensitivitas rotasi dapat diubah secara real-time melalui Inspector
    public float mouseSensitivity = 100f;
    
    // Referensi ke entitas induk (tubuh pemain) untuk rotasi horizontal
    public Transform playerBody;
 
 
    // Variabel pelacak sudut vertikal
    private float xRotation = 0f;

    void Start()
    {
        // Instruksi sistem operasi untuk mengunci dan menyembunyikan kursor di tengah layar game
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. EKSTRAKSI VEKTOR INPUT TETIKUS
        // Time.deltaTime memastikan kecepatan putaran konsisten terlepas dari frame-rate perangkat
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 2. KALKULASI ROTASI VERTIKAL KEMERA (Sumbu X)
        // Kita menguranginya (-=) karena pergerakan mouse ke atas (positif) 
        // secara matematis dalam Unity diterjemahkan sebagai rotasi negatif pada sumbu X
        xRotation -= mouseY;
        
        // Fungsi Clamp mengunci nilai matematis agar leher pemain tidak patah berputar 360 derajat
        // Pandangan dikunci maksimal 90 derajat ke atas dan -90 derajat ke bawah
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Menerapkan matriks rotasi (Quaternion) secara lokal ke Kamera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 3. KALKULASI ROTASI HORIZONTAL TUBUH (Sumbu Y)
        // Mengirimkan instruksi ke entitas Player untuk berputar pada sumbu vertikalnya (Vector3.up)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}