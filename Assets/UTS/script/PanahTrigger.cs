using UnityEngine;

public class PanahTrigger : MonoBehaviour
{
    public float kecepatanMuter = 100f;

    void Update()
    {
        transform.Rotate(0, kecepatanMuter * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Simpan posisi panah sebagai titik respawn
            DeathZoneTrigger.titikRespawn = transform.position;
            
            // 2. SIMPAN ROTASI MOBIL saat mobil masih napak aspal dengan lurus
            DeathZoneTrigger.rotasiRespawn = other.transform.rotation;
            
            // 3. Hilangkan panah
            Destroy(gameObject); 
        }
    }
}