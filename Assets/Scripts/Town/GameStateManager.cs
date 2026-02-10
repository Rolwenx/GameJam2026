using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameProgressState CurrentState { get; private set; }

    public event Action<GameProgressState> OnStateChanged;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        RefreshState();
    }

    public void RefreshState()
    {
        GameProgressState newState = CalculateStateFromFlags();

        if (newState == CurrentState) return;

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
    }

    private GameProgressState CalculateStateFromFlags()
    {
        if (IsLevelCompleted(4)) return GameProgressState.Level4Completed;
        if (IsLevelCompleted(3)) return GameProgressState.Level3Completed;
        if (IsLevelCompleted(2)) return GameProgressState.Level2Completed;
        if (IsLevelCompleted(1)) return GameProgressState.Level1Completed;

        return GameProgressState.None;
    }

    public bool IsLevelCompleted(int levelNumber)
    {
        return PlayerPrefs.GetInt($"Level_{levelNumber}_Completed", 0) == 1;
    }

    public void SetLevelCompleted(int levelNumber)
    {
        PlayerPrefs.SetInt($"Level_{levelNumber}_Completed", 1);
        PlayerPrefs.Save();

        RefreshState();
    }
}