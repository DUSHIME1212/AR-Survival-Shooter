// Scripts/Enemies/EnemyBase.cs
using UnityEngine;
using System.Collections;

namespace ARSurvivalShooter
{
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] protected EnemyData data; // ScriptableObject

        protected Transform player;
        protected int currentHealth;
        protected float attackCooldownTimer;

        protected virtual void Start()
        {
            if (data == null)
            {
                Debug.LogError($"[EnemyBase] {gameObject.name} is missing EnemyData! Please assign it in the inspector.");
                enabled = false;
                return;
            }

            player = Camera.main.transform;
            currentHealth = data.maxHealth;
            
            // Auto-assign tag and layer
            gameObject.tag = "Enemy";
            int characterLayer = LayerMask.NameToLayer("Character");
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            if (characterLayer != -1) gameObject.layer = characterLayer;
            else if (enemyLayer != -1) gameObject.layer = enemyLayer;

            // Apply scale
            float finalScale = data.baseScale * GameSettings.Instance.globalEnemyScale;
            transform.localScale = Vector3.one * finalScale;

            AudioManager.Instance.PlayEnemySpawn();
        }

        protected virtual void Update()
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;
            attackCooldownTimer -= Time.deltaTime;
            BehaviorUpdate(); // subclasses define this
        }

        // Abstract: subclasses MUST implement these
        protected abstract void BehaviorUpdate();
        protected abstract void Attack();

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            // Visual feedback: flash red
            StartCoroutine(FlashRed());

            if (currentHealth <= 0) Die();
        }

        protected virtual void Die()
        {
            Debug.Log($"[EnemyBase] {gameObject.name} died! Awarding {data.scoreValue} points.");
            GameManager.Instance.AddScore(data.scoreValue);
            EnemySpawner.Instance.ReturnEnemy(this); // return to enemy pool
        }

        private IEnumerator FlashRed()
        {
            // Change material color briefly
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var r in renderers) r.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            foreach (var r in renderers) r.material.color = data.normalColor;
        }
    }
}