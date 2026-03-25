using UnityEngine;
using System.Collections; // Pustaka wajib untuk fungsionalitas Coroutine (IEnumerator)

public class Weapon : MonoBehaviour
{
    [Header("Balistik & Referensi")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 100f;
    public Camera fpsCam;

    [Header("Efek Visual & Audio")]
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public AudioClip shootSound;

    [Header("Mekanika Bolt Prosedural")]
    public Transform boltModel;         // Referensi objek 3D bolt
    public float boltBackZ = -0.05f;    // Nilai translasi mundur pada sumbu Z (koreksi arah empiris)
    public float boltReturnSpeed = 25f; // Faktor kecepatan interpolasi kembali ke depan
    
    // Variabel internal untuk merekam koordinat absolut awal
    private Vector3 boltOriginalPos;

    void Start()
    {
        // Perekaman koordinat spasial: Menyimpan posisi awal bolt secara matematis
        // localPosition digunakan agar kalkulasi tidak terganggu saat karakter bergerak di ruang global
        if (boltModel != null)
        {
            boltOriginalPos = boltModel.localPosition;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (muzzleFlash != null) muzzleFlash.Play();
        if (audioSource != null && shootSound != null) audioSource.PlayOneShot(shootSound);

        // --- EKSEKUSI ANIMASI BOLT ---
        if (boltModel != null)
        {
            // Memastikan siklus sebelumnya dihentikan jika penembakan terjadi sangat cepat (rapid fire)
            StopCoroutine("AnimateBoltCycle");
            StartCoroutine("AnimateBoltCycle");
        }

        // 1. MENCARI KOORDINAT TARGET (Vektor Mata)
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        // 2. KALKULASI VEKTOR ARAH BARU (Vektor Laras -> Mata)
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        // 3. INSTANSIASI & EKSEKUSI FISIKA
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.AddForce(direction * bulletForce, ForceMode.Impulse);
        }
    }

    // --- FUNGSI ASINKRON (COROUTINE) ---
    // IEnumerator: Tipe data pengembalian matematis yang memungkinkan fungsi untuk menjeda eksekusinya
    // tanpa membekukan thread utama (frame-rate game tetap berjalan).
    IEnumerator AnimateBoltCycle()
    {
        // TAHAP 1: Snap (Teleportasi instan ke titik mundur)
        boltModel.localPosition = new Vector3(boltOriginalPos.x, boltOriginalPos.y, boltOriginalPos.z + boltBackZ);

        // TAHAP 2: Jeda mikrosekon agar retina mata pemain dapat menangkap frame perubahan
        yield return new WaitForSeconds(0.01f);

        // TAHAP 3: Interpolasi Linier (Lerp) untuk transisi mulus kembali ke depan
        float elapsed = 0f;
        float returnDuration = 0.05f; // Batas waktu maksimal kalkulasi (mencegah infinite loop)

        while (elapsed < returnDuration)
        {
            // Vector3.Lerp menghitung titik di antara posisi saat ini dan posisi awal berdasarkan kecepatan
            boltModel.localPosition = Vector3.Lerp(boltModel.localPosition, boltOriginalPos, boltReturnSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            
            // yield return null menginstruksikan mesin: "hentikan eksekusi di sini, lanjutkan di frame berikutnya"
            yield return null; 
        }

        // TAHAP 4: Kalibrasi Akhir (Memastikan koordinat tidak melenceng akibat pembulatan desimal/floating point)
        boltModel.localPosition = boltOriginalPos;
    }
}