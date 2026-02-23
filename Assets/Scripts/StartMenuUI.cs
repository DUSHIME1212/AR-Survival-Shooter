// Scripts/UI/StartMenuUI.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ARSurvivalShooter
{
    public class StartMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject leaderboardPanel;
        [SerializeField] private TMP_Dropdown difficultyDropdown;

        public void OnStartPressed()
        {
            // Save difficulty to GameSettings before loading
            GameSettings.Instance.SetDifficulty(difficultyDropdown.value);
            SceneManager.LoadScene("Gameplay");
        }

        public void OnLeaderboardPressed()
        {
            leaderboardPanel.SetActive(true);
            LeaderboardUI.Instance.Refresh();
        }
    }
}