using UnityEngine;
using System.Collections; // Wajib buat fitur jeda waktu (Coroutine)

public class FinishLine : MonoBehaviour
{
    [Header("Tarik Panel Layar Hitam Kesini")]
    public CanvasGroup layarHitam;

    public float kecepatanPudar = 0.2f; // Makin kecil angkanya, makin lambat pudarnya

    private void OnTriggerEnter(Collider other)
    {
        // Cek apakah yang nabrak adalah mobil kita (Tag: Player)
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeKeHitam());
        }
    }

    IEnumerator FadeKeHitam()
    {
        // Menambah warna gelap pelan-pelan tiap frame
        while (layarHitam.alpha < 1f)
        {
            layarHitam.alpha += Time.deltaTime * kecepatanPudar;
            yield return null;
        }
        
        // (Opsional) Bikin game berhenti / pause setelah layar gelap total
        Time.timeScale = 0f; 
    }
}