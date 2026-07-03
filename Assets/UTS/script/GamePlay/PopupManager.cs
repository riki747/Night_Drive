using UnityEngine;
using TMPro;
using System.Collections;

public class PopupManager : MonoBehaviour
{
    public GameObject panelPopup;

    private TMP_Text teksPopup;

    public float durasiPopup = 1.5f;

    void Awake()
    {
        teksPopup = panelPopup.GetComponentInChildren<TMP_Text>();

        panelPopup.SetActive(false);
    }

    public void TampilkanPopup(string pesan)
    {
        StopAllCoroutines();
        StartCoroutine(ShowPopup(pesan));
    }

    IEnumerator ShowPopup(string pesan)
    {
        panelPopup.SetActive(true);

        if (teksPopup != null)
        {
            teksPopup.text = pesan;
        }

        yield return new WaitForSeconds(durasiPopup);

        panelPopup.SetActive(false);
    }
}