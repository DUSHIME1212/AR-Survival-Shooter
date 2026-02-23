// Scripts/Data/GameSettings.cs
using UnityEngine;

namespace ARSurvivalShooter
{
    public class GameSettings : MonoBehaviour
    {
        public static GameSettings Instance { get; private set; }

        public enum Difficulty { Easy, Hard }
        public Difficulty CurrentDifficulty { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetDifficulty(int index)
        {
            CurrentDifficulty = (Difficulty)index;
        }

        public float GetSpawnInterval() => CurrentDifficulty == Difficulty.Easy ? 3f : 1.5f;
        public int GetEnemyHealthMultiplier() => CurrentDifficulty == Difficulty.Easy ? 1 : 2;
        public float GetEnemySpeedMultiplier() => CurrentDifficulty == Difficulty.Easy ? 1f : 1.5f;
    }
}