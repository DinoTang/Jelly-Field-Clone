using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Won,
    Lost
}

public class GameManager : Singleton<GameManager>
{
    [Header("Game Manager")]
    [SerializeField] private JellyLevelListSO levelList;
    [SerializeField] private int currentLevelIndex;
    [SerializeField] private GameState gameState = GameState.Playing;
    [SerializeField] private List<JellyGoalData> goals = new();
    [SerializeField] private int coinReward;

    public event Action<JellyGoalData> GoalAmountChanged;
    public event Action<GameState> GameStateChanged;
    public event Action LevelRestarted;

    public JellyLevelSO LevelData => this.levelList.GetLevel(this.currentLevelIndex);
    public int CurrentLevelIndex => this.currentLevelIndex;
    public GameState GameState => this.gameState;
    public List<JellyGoalData> Goals => this.goals;
    public int CoinReward => this.coinReward;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != this) return;

        this.LoadGoals();
    }

    private void LoadGoals()
    {
        this.goals.Clear();

        if (this.LevelData == null || this.LevelData.Goals == null)
            return;

        foreach (JellyGoalData goal in this.LevelData.Goals)
        {
            if (goal == null) continue;

            this.goals.Add(goal.Clone());
        }
    }

    public void SetGameState(GameState newState)
    {
        if (this.gameState == newState) return;

        this.gameState = newState;
        this.GameStateChanged?.Invoke(newState);
    }

    public void RegisterClearedJellyPiece(JellyColor color)
    {
        if (this.gameState != GameState.Playing) return;

        foreach (JellyGoalData goal in this.goals)
        {
            if (goal == null || goal.IsCompleted || goal.Goal == null) continue;
            if (goal.Goal.JellyColor != color) continue;

            goal.DecreaseTargetAmount();
            this.GoalAmountChanged?.Invoke(goal);
            break;
        }

        this.CheckWinState();
    }

    private void CheckWinState()
    {
        if (this.goals.Count == 0) return;

        foreach (JellyGoalData goal in this.goals)
        {
            if (goal != null && !goal.IsCompleted)
                return;
        }

        this.coinReward = this.LevelData.CoinReward;
        this.SetGameState(GameState.Won);
    }

    public void RestartLevel()
    {
        this.LoadLevel(this.currentLevelIndex);
    }

    public void LoadNextLevel()
    {
        if (this.levelList == null) return;

        int nextLevelIndex = this.currentLevelIndex + 1;

        if (nextLevelIndex >= this.levelList.LevelCount)
        {
            Debug.Log("All levels completed.");
            return;
        }

        this.LoadLevel(nextLevelIndex);
    }

    private void LoadLevel(int levelIndex)
    {
        if (this.levelList == null) return;
        if (levelIndex < 0 || levelIndex >= this.levelList.LevelCount) return;

        this.currentLevelIndex = levelIndex;

        this.LoadGoals();
        this.coinReward = 0;

        this.LevelRestarted?.Invoke();

        this.SetGameState(GameState.Playing);

        if (BoardManager.Instance != null)
            BoardManager.Instance.ResetBoard();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
            this.LoadGoals();
    }
}