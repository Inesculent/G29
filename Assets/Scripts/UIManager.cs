using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // ✅ Needed for TextMeshProUGUI

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Screen Effects")]
    public Image screenOverlay; // For fading in/out
    public Image damageFlash;   // For red flash effect
    public float fadeDuration = 1.5f;
    public float damageFlashDuration = 0.2f;

    [Header("Loop Counter UI")]
    public TextMeshProUGUI loopCounterText; // ✅ Assign in Inspector

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ✅ Public method to update loop counter UI
    public void UpdateLoopCounter(int count)
    {
        if (loopCounterText != null)
        {
            loopCounterText.text = $"Loops: {count}";
        }
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(1, 0)); // Fade from black to transparent
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(0, 1)); // Fade from transparent to black
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float elapsedTime = 0f;
        Color color = screenOverlay.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            screenOverlay.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        screenOverlay.color = color;
    }

    public void ShowDamageFlash()
    {
        StartCoroutine(DamageFlashEffect());
    }

    private IEnumerator DamageFlashEffect()
    {
        Color color = damageFlash.color;
        color.a = 0.5f; // Half-transparent red
        damageFlash.color = color;

        yield return new WaitForSeconds(damageFlashDuration);

        color.a = 0f;
        damageFlash.color = color;
    }
}

