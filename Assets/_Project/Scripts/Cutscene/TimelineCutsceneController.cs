using System;
using Game.Flow;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineCutsceneController : FlowComponent
{
    [Header("Timeline")]
    [SerializeField]
    private PlayableDirector director;

    public event Action Started;
    public event Action Finished;

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        if (director == null)
            director = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        if (director != null)
            director.stopped += HandleTimelineStopped;
    }

    private void OnDisable()
    {
        if (director != null)
            director.stopped -= HandleTimelineStopped;
    }

    public void Play()
    {
        if (IsPlaying)
            return;

        if (director == null)
        {
            Debug.LogError($"{name} tidak memiliki PlayableDirector.");
            return;
        }

        IsPlaying = true;

        Started?.Invoke();

        director.Play();
    }

    public void Stop()
    {
        if (!IsPlaying)
            return;

        director.Stop();
    }

    public void RestoreCompleted()
    {
        if (director == null)
            return;

        director.time = director.duration;

        // Evaluasi semua track ke frame terakhir
        director.Evaluate();

        // Pastikan tidak sedang play
        director.Stop();

        IsPlaying = false;
    }

    private void HandleTimelineStopped(PlayableDirector director)
    {
        if (!IsPlaying)
            return;

        IsPlaying = false;

        Finished?.Invoke();
    }
}