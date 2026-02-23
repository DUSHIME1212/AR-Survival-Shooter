// Scripts/UI/StartMenuUI.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ARSurvivalShooter
{
    public class StartMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject startMenuPanel;
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
            startMenuPanel.SetActive(false);
            leaderboardPanel.SetActive(true);
            leaderboardPanel.GetComponent<LeaderboardUI>().Refresh();
        }

        public void OnBackPressed()
        {
            leaderboardPanel.SetActive(false);
            startMenuPanel.SetActive(true);
        }
    }
}