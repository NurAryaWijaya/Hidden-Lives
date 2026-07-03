using System;
using UnityEngine;

public enum GameState
{
    Exploration,
    Dialogue,
    Pause,
    Cutscene
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState CurrentState { get; private set; } = GameState.Exploration;

    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        OnStateChanged?.Invoke(CurrentState);
    }

    public bool IsState(GameState state)
    {
        return CurrentState == state;
    }
}