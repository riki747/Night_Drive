using UnityEngine;
using UnityEngine.SceneManagement; // Wajib buat fitur Replay / Quit

public class PauseMenuManager : MonoBehaviour
{
    [Header("Tarik 'panel Pause' ke sini")]
    public GameObject panelPause;

    void Start()
    {
        Time.timeScale = 1f; // Pastikan waktu normal saat game mulai
        if (panelPause != null)
        {
            panelPause.SetActive(false); // Sembunyikan panel pause di awal
        }
    }

    // Fungsi untuk tombol Pause (tombol || di pojok kanan atas)
    public void PauseGame()
    {
        if (panelPause != null) panelPause.SetActive(true);
        Time.timeScale = 0f; // Bekukan waktu dan fisika mobil!
    }

    // Fungsi untuk tombol "Resume"
    public void ResumeGame()
    {
        if (panelPause != null) panelPause.SetActive(false);
        Time.timeScale = 1f; // Jalankan waktu kembali
    }

    // Fungsi untuk tombol "Replay"
    public void ReplayGame()
    {
        Time.timeScale = 1f; // Waktu harus dinormalkan dulu sebelum ngulang scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Fungsi untuk tombol "Quit"
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Keluar dari Game!");
        Application.Quit(); // Ini akan menutup game jika sudah di-build jadi .exe
        
        // Catatan: Jika lu punya scene Main Menu, lu bisa ganti Application.Quit() jadi:
        // SceneManager.LoadScene("NamaSceneMainMenuLu");
    }
}