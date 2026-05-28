using UnityEngine;

public class cursorLockNight : MonoBehaviour // <-- NAMA CLASS DISAMAKAN DENGAN NAMA FILE
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     Cursor.lockState = CursorLockMode.Locked;
     Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
}