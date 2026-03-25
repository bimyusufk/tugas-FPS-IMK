using UnityEngine;
using TMPro; // Pustaka wajib untuk mengontrol TextMeshPro

public class GameManager : MonoBehaviour
{
    // 1. DEKLARASI SINGLETON MATEMATIS
    public static GameManager instance;

    // 2. VARIABEL REFERENSI
    public GameObject targetPrefab;    // Referensi cetak biru musuh
    public Transform playerTransform;  // Referensi pusat radius (Pemain)
    public TextMeshProUGUI scoreText;  // Referensi teks UI

    // 3. PARAMETER SISTEM
    public float spawnRadius = 20f;    // Radius area pemunculan (meter)
    public int maxTargets = 5;         // Batas maksimum entitas
    
    private int currentScore = 0;
    private int currentTargets = 0;

    void Awake()
    {
        // Mengunci arsitektur Singleton: Hanya boleh ada satu GameManager
        if (instance == null) {
            instance = this;
        }
    }

    void Start()
    {
        // Memunculkan target awal hingga batas maksimum saat game dimulai
        for (int i = 0; i < maxTargets; i++)
        {
            SpawnTarget();
        }
        UpdateScoreUI();
    }

    public void SpawnTarget()
    {
        // Kalkulasi vektor spasial: Mencari titik acak di dalam lingkaran 2D (X, Y)
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        
        // Mentranslasikan lingkaran 2D ke lantai 3D (X, Z). 
        // Y dikunci di angka 1 agar kotak tidak tenggelam ke dalam lantai.
        Vector3 spawnPosition = playerTransform.position + new Vector3(randomCircle.x, 1f, randomCircle.y);

        // Eksekusi Instansiasi (Pembuatan objek)
        Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        currentTargets++;
    }

    // Fungsi ini akan dipanggil oleh target saat ia hancur
    public void TargetDestroyed()
    {
        currentTargets--;
        currentScore += 10; // Menambah 10 poin per kill
        UpdateScoreUI();

        // Evaluasi kondisi: Jika jumlah target di bawah batas maksimum, buat baru
        if (currentTargets < maxTargets)
        {
            SpawnTarget();
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "SCORE: " + currentScore.ToString();
    }
}