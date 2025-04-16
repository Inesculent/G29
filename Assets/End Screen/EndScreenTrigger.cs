using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingTrigger : MonoBehaviour
{
    public Image fadeOverlay;
    public float fadeDuration = 2f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeIn());
        }
    }
    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeOverlay.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeOverlay.color = new Color(color.r, color.g, color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeOverlay.color = new Color(color.r, color.g, color.b, 1f);
        SceneManager.LoadScene("EndingScene");
    }
}

