using UnityEngine;
using TMPro; // Pakai TextMeshPro

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance; // Biar gampang dipanggil dari skrip lain

    [Header("UI Tutorial")]
    public GameObject panelTutorial; 
    public TextMeshProUGUI teksTutorial;
    
    [Header("Analog Joystick")]
    public FixedJoystick joystick; // Tarik Fixed Joystick ke sini

    // Daftar status kita lagi nunggu apa
    public enum Langkah { TidakAda, TungguGas, TungguRem, TungguAnalog }
    public Langkah langkahSekarang = Langkah.TidakAda;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // 1. Saat game baru mulai, langsung munculkan tutorial GAS!
        TampilkanPesan("Tahan tombol GAS untuk melaju!", Langkah.TungguGas);
    }

    private void Update()
    {
        // Khusus ANALOG: kita cek di sini karena analog itu digeser, bukan diklik
        if (langkahSekarang == Langkah.TungguAnalog && joystick != null)
        {
            // Jika analog digeser ke kanan atau kiri sedikit saja
            if (Mathf.Abs(joystick.Horizontal) > 0.2f) 
            {
                TutupTutorial();
            }
        }
    }

    // Fungsi untuk memunculkan panel dan pause game
    public void TampilkanPesan(string pesan, Langkah langkah)
    {
        langkahSekarang = langkah;
        teksTutorial.text = pesan;
        panelTutorial.SetActive(true);
        
        Time.timeScale = 0f; // Pause game!
    }

    // Fungsi untuk menghilangkan panel dan play game
    private void TutupTutorial()
    {
        langkahSekarang = Langkah.TidakAda;
        panelTutorial.SetActive(false);
        
        Time.timeScale = 1f; // Lanjut jalan!
    }

    // ===============================================
    // FUNGSI INI AKAN KITA SAMBUNGKAN KE TOMBOL UI
    // ===============================================
    public void CekGasDitekan()
    {
        if (langkahSekarang == Langkah.TungguGas)
        {
            TutupTutorial(); // Jika disuruh gas dan dipencet gas, tutup pop-up!
        }
    }

    public void CekRemDitekan()
    {
        if (langkahSekarang == Langkah.TungguRem)
        {
            TutupTutorial(); // Jika disuruh rem dan dipencet rem, tutup pop-up!
        }
    }
}