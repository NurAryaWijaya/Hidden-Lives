using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComputerProgressBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider progressBar;

    [Header("Progress")]
    [SerializeField] private float progressDuration = 8f;

    [Header("Movement")]
    [SerializeField] private float minStep = 0.01f;
    [SerializeField] private float maxStep = 0.08f;

    [Header("Speed")]
    [SerializeField] private float minSpeed = 0.15f;
    [SerializeField] private float maxSpeed = 0.5f;

    [Header("Pause")]
    [SerializeField] private float minPause = 0.2f;
    [SerializeField] private float maxPause = 1.5f;

    [Header("Long Stuck")]
    [Range(0f, 1f)]
    [SerializeField] private float longPauseChance = 0.15f;

    [SerializeField] private float minLongPause = 1f;
    [SerializeField] private float maxLongPause = 3f;

    private Coroutine progressCoroutine;

    public bool IsReady { get; set; } = false;


    public void StartProgress()
    {
        if (progressCoroutine != null)
            StopCoroutine(progressCoroutine);

        progressCoroutine = StartCoroutine(ProgressRoutine());
    }

    public void StopProgress()
    {
        if (progressCoroutine != null)
        {
            StopCoroutine(progressCoroutine);
            progressCoroutine = null;
        }
    }

    private IEnumerator ProgressRoutine()
    {
        progressBar.value = 0f;
        IsReady = false;

        float elapsedTime = 0f;

        while (elapsedTime < progressDuration)
        {
            float remainingTime = progressDuration - elapsedTime;


            if (remainingTime <= 0.5f)
            {
                // Selesaikan progress secara halus
                float startValue = progressBar.value;
                float startTime = elapsedTime;

                while (elapsedTime < progressDuration)
                {
                    elapsedTime += Time.deltaTime;

                    float t = Mathf.InverseLerp(
                        startTime,
                        progressDuration,
                        elapsedTime
                    );

                    // SmoothStep agar tidak terlihat mendadak
                    float smoothT = Mathf.SmoothStep(0f, 1f, t);

                    progressBar.value = Mathf.Lerp(
                        startValue,
                        1f,
                        smoothT
                    );

                    yield return null;
                }

                break;
            }


            float step = Random.Range(
                minStep,
                maxStep
            );

            // 20% kemungkinan langkah lebih besar
            if (Random.value < 0.2f)
            {
                step *= Random.Range(
                    1.5f,
                    2.5f
                );
            }

            float target = Mathf.Min(
                progressBar.value + step,
                0.98f
            );

            float speed = Random.Range(
                minSpeed,
                maxSpeed
            );

            while (progressBar.value < target)
            {
                float deltaTime = Time.deltaTime;

                progressBar.value = Mathf.MoveTowards(
                    progressBar.value,
                    target,
                    speed * deltaTime
                );

                elapsedTime += deltaTime;

                // Jangan sampai waktu terlewati
                if (elapsedTime >= progressDuration)
                    break;

                yield return null;
            }


            // Jika waktu habis
            if (elapsedTime >= progressDuration)
                break;

            float pause;

            if (Random.value < longPauseChance)
            {
                // Long stuck
                pause = Random.Range(
                    minLongPause,
                    maxLongPause
                );
            }
            else
            {
                // Pause normal
                pause = Random.Range(
                    minPause,
                    maxPause
                );
            }

            pause = Mathf.Min(
                pause,
                remainingTime - 0.5f
            );

            pause = Mathf.Max(
                pause,
                0f
            );

            elapsedTime += pause;

            yield return new WaitForSeconds(pause);
        }


        progressBar.value = 1f;

        progressCoroutine = null;

        IsReady = true;
    }

    public void ResetProgress()
    {
        StopProgress();

        progressBar.value = 0f;
        IsReady = false;
    }
}
