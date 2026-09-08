using System.Collections;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CanvasGroup fadeCanvas;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float blackDuration = 3f;

    private Coroutine transitionCoroutine;
    private bool isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayTransition()
    {
        if (isTransitioning)
            return;

        transitionCoroutine = StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        isTransitioning = true;

        // Pastikan mulai dari transparan
        fadeCanvas.alpha = 0f;

        // ==============================
        // FADE TO BLACK
        // ==============================

        yield return Fade(0f, 1f);

        // ==============================
        // LAYAR SEKARANG HITAM
        // ==============================

        // TransitionManager TIDAK melakukan
        // LoadScene atau memanggil script lain.
        //
        // Script / Flow / Node lain bebas
        // melakukan apa pun di belakang layar.

        yield return new WaitForSeconds(blackDuration);

        // ==============================
        // FADE FROM BLACK
        // ==============================

        yield return Fade(1f, 0f);

        isTransitioning = false;
        transitionCoroutine = null;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;

        fadeCanvas.alpha = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / fadeDuration);

            fadeCanvas.alpha = Mathf.Lerp(from, to, t);

            yield return null;
        }

        fadeCanvas.alpha = to;
    }
}