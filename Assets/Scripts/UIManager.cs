// Scripts/UI/UIManager.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace ARSurvivalShooter
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Panels")]
        [SerializeField] private GameObject hudPanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject placementPanel; // "Tap to place" UI

        [Header("Buttons")]
        [SerializeField] private Button placeButton;

        [Header("HUD Elements")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text timerText;

        private void Start()
        {
            // Ensure placement panel is active and others are hidden
            placementPanel.SetActive(true);
            hudPanel.SetActive(false);
            gameOverPanel.SetActive(false);

            // Programmatically link the button to the placement logic
            if (placeButton != null)
            {
                placeButton.onClick.AddListener(() => {
                    if (ARPlacementController.Instance != null)
                        ARPlacementController.Instance.PlaceGame();
                });
            }
        }

        [Header("Game Over Elements")]
        [SerializeField] private TMP_Text finalScoreText;
        [SerializeField] private TMP_Text enemiesKilledText;
        [SerializeField] private TMP_Text timeSurvivedText;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void OnEnable()
        {
            PlayerHealth.OnHealthChanged += UpdateHealth;
            GameManager.OnGameStarted += OnGameStarted;
        }

        private void OnDisable()
        {
            PlayerHealth.OnHealthChanged -= UpdateHealth;
            GameManager.OnGameStarted -= OnGameStarted;
        }

        private void OnGameStarted()
        {
            placementPanel.SetActive(false);
            hudPanel.SetActive(true);
        }

        public void UpdateHealth(int current, int max) =>
            healthBar.value = (float)current / max;

        public void UpdateScore(int score) =>
            scoreText.text = $"Score: {score}";

        public void UpdateTimer(float time)
        {
            int t = Mathf.Max(0, Mathf.CeilToInt(time));
            timerText.text = $"{t / 60:00}:{t % 60:00}";
        }

        public void ShowGameOver(int score, int kills, float time)
        {
            hudPanel.SetActive(false);
            gameOverPanel.SetActive(true);
            finalScoreText.text = $"Score: {score}";
            enemiesKilledText.text = $"Enemies: {kills}";
            timeSurvivedText.text = $"Time: {time:F1}s";
        }

        public void OnRestartPressed() => SceneManager.LoadScene("Gameplay");
        public void OnMainMenuPressed() => SceneManager.LoadScene("StartMenu");
    }
}
