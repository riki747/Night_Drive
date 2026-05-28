using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // WAJIB ADA INI BIAR BISA PINDAH SCENE!

public class FinishLineNext : MonoBehaviour
{
    [Header("Tarik Panel Layar Hitam Kesini")]
    public CanvasGroup layarHitam;
    public float kecepatanPudar = 1f; 
    
    [Header("Nama Scene Berikutnya (Ketik Manual)")]
    public string namaSceneSelanjutnya = "Driver Night 2"; // Pastikan namanya persis sama dengan file Scene lu!

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeKeHitamLaluPindah());
        }
    }

    IEnumerator FadeKeHitamLaluPindah()
    {
        // 1. Proses memudarkan layar jadi hitam pekat
        while (layarHitam.alpha < 1f)
        {
            layarHitam.alpha += Time.deltaTime * kecepatanPudar;
            yield return null;
        }
        
        // 2. Jeda setengah detik pas layar udah gelap total (Biar dramatis)
        yield return new WaitForSeconds(0.5f); 
        
        // 3. BOOM! Pindah ke Scene 2
        SceneManager.LoadScene(namaSceneSelanjutnya); 
    }
}