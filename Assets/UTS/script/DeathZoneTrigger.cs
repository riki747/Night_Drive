using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZoneTrigger : MonoBehaviour
{
    public static Vector3 titikRespawn;
    public static Quaternion rotasiRespawn; // Tambahan: Variabel penyimpan rotasi/kemiringan

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            titikRespawn = player.transform.position;
            rotasiRespawn = player.transform.rotation; // Simpan rotasi datar pas game baru mulai
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 1. Matikan fisika sejenak biar momentum jatuhnya hilang total
                rb.isKinematic = true;  
                
                // 2. Pindahkan posisi ke checkpoint
                other.transform.position = titikRespawn;
                
                // 3. KEMBALIKAN ROTASI menjadi datar siap jalan!
                other.transform.rotation = rotasiRespawn; 
                
                // 4. Nyalakan fisika lagi
                rb.isKinematic = false; 
            }
        }
    }
}