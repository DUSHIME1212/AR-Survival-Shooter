// Scripts/Managers/LeaderboardManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ARSurvivalShooter
{
    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance { get; private set; }
        private const string SaveKey = "LeaderboardData";
        private const int MaxSessions = 5;

        private LeaderboardData data;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load();
        }

        public void SaveSession(int score, int kills, float time)
        {
            data.sessions.Add(new SessionData
            {
                score = score,
                enemiesKilled = kills,
                timeSurvived = time,
                date = System.DateTime.Now.ToString("MM/dd HH:mm")
            });

            // Sort by score descending, keep only top 5
            data.sessions = data.sessions
                .OrderByDescending(s => s.score)
                .Take(MaxSessions)
                .ToList();

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public List<SessionData> GetSessions() => data.sessions;

        private void Load()
        {
            string json = PlayerPrefs.GetString(SaveKey, "{}");
            data = JsonUtility.FromJson<LeaderboardData>(json) ?? new LeaderboardData();
        }
    }
}