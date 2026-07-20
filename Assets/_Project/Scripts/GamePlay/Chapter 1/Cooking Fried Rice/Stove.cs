using Game.Audio;
using Game.Flow;
using System.Collections;
using UnityEngine;


public class Stove : Interactable, IFlowActivatable
{
    [SerializeField]
    private GameObject stoveKnob;

    [SerializeField] private float rotateDuration = 0.3f;

    private bool isStoveOn;

    private Coroutine rotateCoroutine;
    private Quaternion knobOffRotation;
    private Quaternion knobOnRotation;

    // Audio
    [Header("SFX")]
    [SerializeField] private AudioSource stoveAudioSource;

    [SerializeField] private AudioClip stoveOnSFX;
    [SerializeField] private AudioClip stoveOffSFX;
    [SerializeField] private AudioClip cookingLoopSFX;

    [Header("Cooking Audio")]
    [SerializeField, Range(0f, 1f)]
    private float cookingVolume = 0.3f;

    [Header("Cooking Loop ")]
    [SerializeField] private float cookingStartDelay = 3f;
    [SerializeField] private float cookingFadeInDuration = 2f;
    [SerializeField] private float cookingStopDelay = 5f;
    [SerializeField] private float cookingFadeOutDuration = 2f;

    private Coroutine cookingCoroutine;

    [Header("Flame Light")]
    [SerializeField] private Light flameLight;

    [SerializeField] private float baseIntensity = 0.005f;
    [SerializeField] private float intensityVariation = 0.0005f;
    [SerializeField] private float flickerSpeed = 5f;

    private Coroutine flickerCoroutine;

    private void Start()
    {
        knobOffRotation = stoveKnob.transform.localRotation;
        knobOnRotation = knobOffRotation * Quaternion.Euler(0, 0, 90);

        stoveAudioSource.playOnAwake = false;
        stoveAudioSource.loop = true;
        stoveAudioSource.clip = cookingLoopSFX;

        stoveAudioSource.volume = cookingVolume;
    }

    public override void Interact(PlayerInteractor player)
    {
        if (!isStoveOn)
        {
            StoveOn();
        } else
        {
            StoveOff();
        }
    }

    private void StoveOn()
    {
        Debug.Log("Stove On");

        RotateTo(knobOnRotation);

        AudioManager.Instance.PlaySFX(stoveOnSFX);

        if (cookingCoroutine != null)
            StopCoroutine(cookingCoroutine);

        cookingCoroutine = StartCoroutine(StartCookingLoop());

        // nyalakan lampu api
        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);

        flameLight.enabled = true;
        flickerCoroutine = StartCoroutine(FlickerLight());

        FlowEventManager.Instance.Raise("Stove_On");

        isStoveOn = true;
    }

    private void StoveOff()
    {
        Debug.Log("Stove Off");

        RotateTo(knobOffRotation);

        AudioManager.Instance.PlaySFX(stoveOffSFX);

        if (cookingCoroutine != null)
            StopCoroutine(cookingCoroutine);

        cookingCoroutine = StartCoroutine(StopCookingLoop());

        // matikan flicker
        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);

        flameLight.enabled = false;

        FlowEventManager.Instance.Raise("Stove_Off");

        isStoveOn = false;
    }

    // Api
    private IEnumerator FlickerLight()
    {
        while (true)
        {
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);

            flameLight.intensity = Mathf.Lerp(
                baseIntensity - intensityVariation,
                baseIntensity + intensityVariation,
                noise);

            yield return null;
        }
    }

    // SFX Delay
    private IEnumerator StartCookingLoop()
    {
        yield return new WaitForSeconds(cookingStartDelay);

        stoveAudioSource.volume = 0f;
        stoveAudioSource.Play();

        float t = 0f;

        while (t < cookingFadeInDuration)
        {
            t += Time.deltaTime;

            stoveAudioSource.volume =
                Mathf.Lerp(0f, cookingVolume, t / cookingFadeInDuration);

            yield return null;
        }

        stoveAudioSource.volume = cookingVolume;
    }

    private IEnumerator StopCookingLoop()
    {
        yield return new WaitForSeconds(cookingStopDelay);

        float startVolume = stoveAudioSource.volume;

        float t = 0f;

        while (t < cookingFadeOutDuration)
        {
            t += Time.deltaTime;

            stoveAudioSource.volume =
                Mathf.Lerp(startVolume, 0f, t / cookingFadeOutDuration);

            yield return null;
        }

        stoveAudioSource.Stop();
        stoveAudioSource.volume = 1f;
    }

    // Rotasi Mesh
    private void RotateTo(Quaternion target)
    {
        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateKnob(target));
    }

    private IEnumerator RotateKnob(Quaternion targetRotation)
    {
        Quaternion startRotation = stoveKnob.transform.localRotation;

        float time = 0f;

        while (time < rotateDuration)
        {
            time += Time.deltaTime;

            stoveKnob.transform.localRotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                time / rotateDuration);

            yield return null;
        }

        stoveKnob.transform.localRotation = targetRotation;
    }

    // Aktivasi
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void Activate()
    {
        int interactableLayer = LayerMask.NameToLayer("Interactable");

        if (interactableLayer == -1)
        {
            return;
        }

        SetLayerRecursively(gameObject, interactableLayer);
    }

    public void Deactivate()
    {
        int heldObjectLayer = LayerMask.NameToLayer("HeldObject");

        if (heldObjectLayer == -1)
        {
            return;
        }

        SetLayerRecursively(gameObject, heldObjectLayer);
    }
}
