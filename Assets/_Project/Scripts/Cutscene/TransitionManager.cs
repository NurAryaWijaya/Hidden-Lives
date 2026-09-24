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

        fadeCanvas.alpha = 0f;

        yield return Fade(0f, 1f);


        yield return new WaitForSeconds(blackDuration);

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