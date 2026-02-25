// Scripts/Player/PlayerHealth.cs
using UnityEngine;
using System;

namespace ARSurvivalShooter
{
    public class PlayerHealth : MonoBehaviour
    {
        public static event Action OnPlayerDied;
        public static event Action<int, int> OnHealthChanged; // current, max

        [SerializeField] private int maxHealth = 100;
        private int currentHealth;

        private void Awake()
        {
            // Auto-assign tag and layer
            gameObject.tag = "Player";
            int characterLayer = LayerMask.NameToLayer("Character");
            int playerLayer = LayerMask.NameToLayer("Player");
            if (characterLayer != -1) gameObject.layer = characterLayer;
            else if (playerLayer != -1) gameObject.layer = playerLayer;
        }

        private void OnEnable()
        {
            currentHealth = maxHealth;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void TakeDamage(int amount)
        {
            currentHealth = Mathf.Max(0, currentHealth - amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            AudioManager.Instance.PlayMeleeAttack(); // play damage sound

            if (currentHealth <= 0)
            {
                AudioManager.Instance.PlayPlayerDeath();
                OnPlayerDied?.Invoke();
                GameManager.Instance.TriggerGameOver();
            }
        }
    }
}