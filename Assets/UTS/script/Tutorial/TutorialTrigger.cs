using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Pengaturan Tutorial")]
    [TextArea]
    public string pesanTutorial;
    
    // Pilih ini trigger untuk nyuruh Rem atau nyuruh Analog??
    public TutorialManager.Langkah langkahYangDisuruh; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Panggil Tutorial Manager untuk memunculkan teks
            if (TutorialManager.instance != null)
            {
                TutorialManager.instance.TampilkanPesan(pesanTutorial, langkahYangDisuruh);
            }
            
            // Hapus kotak sensor ini agar tidak terpicu 2 kali
            Destroy(gameObject);
        }
    }
}