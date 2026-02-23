// Scripts/Managers/GameManager.cs
using UnityEngine;
using System;

namespace ARSurvivalShooter
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum GameState { PlacingGame, Playing, Paused, GameOver }
        public GameState CurrentState { get; private set; }

        // Events other systems subscribe to
        public static event Action OnGameStarted;
        public static event Action<int, int, float> OnGameOver; // score, kills, time

        [Header("Settings")]
        [SerializeField] private float gameDuration = 60f;

        private float timeRemaining;
        private int score;
        private int enemiesKilled;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Start()
        {
            // Auto-start game when Gameplay scene loads.
            // If you have an AR placement step, call StartGame() from
            // ARPlacementController instead and remove this Start() call.
            StartGame();
        }

        public void StartGame()
        {
            CurrentState = GameState.Playing;
            timeRemaining = gameDuration;
            score = 0;
            enemiesKilled = 0;
            OnGameStarted?.Invoke();
        }

        private void Update()
        {
            if (CurrentState != GameState.Playing) return;

            timeRemaining -= Time.deltaTime;
            UIManager.Instance.UpdateTimer(timeRemaining);

            if (timeRemaining <= 0f) TriggerGameOver();
        }

        public void AddScore(int points)
        {
            score += points;
            enemiesKilled++;
            UIManager.Instance.UpdateScore(score);
        }

        public void TriggerGameOver()
        {
            if (CurrentState == GameState.GameOver) return;
            CurrentState = GameState.GameOver;

            if (EnemySpawner.Instance != null)
            {
                EnemySpawner.Instance.StopSpawning();
                EnemySpawner.Instance.DestroyAllEnemies();
            }

            float timeSurvived = gameDuration - timeRemaining;

            if (LeaderboardManager.Instance != null)
                LeaderboardManager.Instance.SaveSession(score, enemiesKilled, timeSurvived);

            OnGameOver?.Invoke(score, enemiesKilled, timeSurvived);

            if (UIManager.Instance != null)
                UIManager.Instance.ShowGameOver(score, enemiesKilled, timeSurvived);
        }
    }
}