using UnityEngine;
using UnityEngine.UI; // Wajib ditambahkan untuk mengontrol UI Gambar,

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("UI Pengaturan Suara")]
    [SerializeField] private Image ikonTombolSuara; // Wadah untuk naruh komponen Image tombol
    [SerializeField] private Sprite gambarSuaraNyala;  // Gambar logo speaker biasa
    [SerializeField] private Sprite gambarSuaraMati;   // Gambar logo speaker dicoret

    private bool isMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 1. Ambil data dari memori. (0 = Suara Nyala, 1 = Suara Mati).
        isMuted = PlayerPrefs.GetInt("GameMuted", 0) == 1;
        
        // 2. Terapkan status suara (nyala/mati) secara sistem
        ApplyMuteStatus();

        // 3. Langsung perbarui gambar tombol saat game baru mulai
        UpdateGambarTombol();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void ToggleMute()
    {
        // Membalikkan status (Kalau nyala jadi mati, kalau mati jadi nyala)
        isMuted = !isMuted;
        
        // Simpan status baru ini ke memori hape/PC
        PlayerPrefs.SetInt("GameMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // Terapkan ke sistem dan perbarui gambar
        ApplyMuteStatus();
        UpdateGambarTombol();
    }

    private void ApplyMuteStatus()
    {
        // Menggunakan manipulasi volume global agar suara yang dipicu saat mute
        // tetap berjalan sampai habis (tidak nyangkut/tertahan).
        if (isMuted)
        {
            AudioListener.volume = 0f; // Bisu total
        }
        else
        {
            AudioListener.volume = 1f; // Volume normal (100%)
        }
    }

    private void UpdateGambarTombol()
    {
        // Pastikan slot-slot UI di Inspector sudah diisi biar nggak error
        if (ikonTombolSuara != null && gambarSuaraNyala != null && gambarSuaraMati != null)
        {
            if (isMuted)
            {
                ikonTombolSuara.sprite = gambarSuaraMati; // Ganti jadi logo dicoret
            }
            else
            {
                ikonTombolSuara.sprite = gambarSuaraNyala; // Ganti jadi logo nyala
            }
        }
    }
}