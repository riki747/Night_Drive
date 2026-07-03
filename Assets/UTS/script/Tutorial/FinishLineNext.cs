using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinishLineNext : MonoBehaviour
{
    [Header("Fade")]
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    [Header("Loading Text")]
    public TMP_Text loadingText;

    [Header("Scene Selanjutnya")]
    public string namaSceneSelanjutnya = "Night Drive 1";

    private bool selesai = false;

    private void Start()
    {
        // Fade transparan
        Color c = fadeImage.color;
        c.a = 0;
        fadeImage.color = c;

        // Sembunyikan tulisan
        if (loadingText != null)
            loadingText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (selesai) return;

        if (other.CompareTag("Player"))
        {
            selesai = true;

            // Tandai tutorial selesai
            PlayerPrefs.SetInt("TutorialCompleted", 1);
            PlayerPrefs.Save();

            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
{
    fadeImage.transform.SetAsLastSibling();
    loadingText.transform.SetAsLastSibling();

    Color c = fadeImage.color;

    while (c.a < 1f)
    {
        c.a += Time.deltaTime * fadeSpeed;
        c.a = Mathf.Clamp01(c.a);
        fadeImage.color = c;

        yield return null;
    }

    loadingText.gameObject.SetActive(true);

    yield return new WaitForSeconds(1.5f);

    SceneManager.LoadScene(namaSceneSelanjutnya);
}
}