// Scripts/UI/LeaderboardUI.cs
using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace ARSurvivalShooter
{
    public class LeaderboardUI : MonoBehaviour
    {
        public static LeaderboardUI Instance { get; private set; }

        [SerializeField] private Transform entriesParent;
        [SerializeField] private GameObject entryPrefab; // a UI row prefab with TMP_Text children

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void Refresh()
        {
            // Clear old entries
            foreach (Transform child in entriesParent)
                Destroy(child.gameObject);

            // Populate from LeaderboardManager
            List<SessionData> sessions = LeaderboardManager.Instance.GetSessions();
            for (int i = 0; i < sessions.Count; i++)
            {
                GameObject entry = Instantiate(entryPrefab, entriesParent);
                TMP_Text[] texts = entry.GetComponentsInChildren<TMP_Text>();
                if (texts.Length >= 4)
                {
                    texts[0].text = $"#{i + 1}";
                    texts[1].text = $"Score: {sessions[i].score}";
                    texts[2].text = $"Kills: {sessions[i].enemiesKilled}";
                    texts[3].text = $"Time: {sessions[i].timeSurvived:F1}s";
                }
            }
        }
    }
}
