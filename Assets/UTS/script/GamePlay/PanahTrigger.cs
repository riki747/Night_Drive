using UnityEngine;

public class PanahTrigger : MonoBehaviour
{
    public float kecepatanMuter = 100f;

    [Header("Pesan Tutorial")]
    [TextArea(2,5)]
    public string pesanTutorial;

    public PopupManager popupManager;

    [Header("Audio")]
    public AudioClip suaraCheckpoint; // Variabel ini wajib ada buat wadah suaranya

    void Update()
    {
        transform.Rotate(0, kecepatanMuter * Time.deltaTime, 0);
    }

    // Cukup SATU fungsi OnTriggerEnter saja yang dipakai
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Panggil suara lewat AudioManager (biar bisa di-mute)
            if (suaraCheckpoint != null && AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(suaraCheckpoint);
            }

            // Simpan posisi checkpoint
            DeathZoneTrigger.titikRespawn = transform.position;

            // Simpan rotasi mobil
            DeathZoneTrigger.rotasiRespawn = other.transform.rotation;

            // Tampilkan popup
            if (popupManager != null)
            {
                popupManager.TampilkanPopup(pesanTutorial);
            }

            // Hapus checkpoint
            Destroy(gameObject);
        }
    }
}