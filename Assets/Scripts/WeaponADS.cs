using UnityEngine;

public class WeaponADS : MonoBehaviour
{
    [Header("Referensi Objek")]
    public Transform weaponModel; // Model fisik senjata yang akan digerakkan
    public Camera fpsCam;         // Kamera pemain untuk efek zoom

    [Header("Koordinat Spasial (Lokal)")]
    public Vector3 hipfirePosition; // Posisi santai (otomatis direkam saat mulai)
    public Vector3 adsPosition;     // Posisi saat iron sight di tengah layar

    [Header("Parameter Optik & Fisika")]
    public float normalFOV = 60f;
    public float adsFOV = 40f;
    public float aimSpeed = 10f;    // Kecepatan transisi animasi

    void Start()
    {
        // Menyimpan posisi awal senjata sebagai posisi Hipfire secara otomatis
        if (weaponModel != null)
        {
            hipfirePosition = weaponModel.localPosition;
        }
    }

    void Update()
    {
        // Input.GetButton("Fire2") mendeteksi tahanan tombol Klik Kanan Mouse
        if (Input.GetButton("Fire2"))
        {
            // EKSEKUSI ADS: Lerp menuju posisi tengah dan FOV sempit
            weaponModel.localPosition = Vector3.Lerp(weaponModel.localPosition, adsPosition, aimSpeed * Time.deltaTime);
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, adsFOV, aimSpeed * Time.deltaTime);
        }
        else
        {
            // EKSEKUSI HIPFIRE: Lerp kembali ke posisi awal dan FOV normal
            weaponModel.localPosition = Vector3.Lerp(weaponModel.localPosition, hipfirePosition, aimSpeed * Time.deltaTime);
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView, normalFOV, aimSpeed * Time.deltaTime);
        }
    }
}