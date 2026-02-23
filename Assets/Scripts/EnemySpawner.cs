// Scripts/Enemies/EnemySpawner.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ARSurvivalShooter
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }

        [SerializeField] private EnemyBase meleeEnemyPrefab;
        [SerializeField] private EnemyBase shooterEnemyPrefab;
        [SerializeField] private float spawnInterval = 3f;
        [SerializeField] private float spawnRadius = 2f; // around game world center

        private List<EnemyBase> activeEnemies = new List<EnemyBase>();
        private Coroutine spawnRoutine;
        private Transform gameWorldCenter;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        public void Initialize(Transform worldCenter)
        {
            gameWorldCenter = worldCenter;
            spawnRoutine = StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            // Spawn on random point around the game world center at plane level
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius), 0,
                Random.Range(-spawnRadius, spawnRadius));
            Vector3 spawnPos = gameWorldCenter.position + randomOffset;

            // 40% chance shooter, 60% melee (adjust for difficulty)
            EnemyBase prefab = Random.value < 0.4f ? shooterEnemyPrefab : meleeEnemyPrefab;
            EnemyBase enemy = Instantiate(prefab, spawnPos, Quaternion.identity, gameWorldCenter);
            activeEnemies.Add(enemy);
        }

        public void ReturnEnemy(EnemyBase enemy)
        {
            activeEnemies.Remove(enemy);
            Destroy(enemy.gameObject); // or use an enemy pool for extra credit
        }

        public void DestroyAllEnemies()
        {
            foreach (var e in activeEnemies)
                if (e != null) Destroy(e.gameObject);
            activeEnemies.Clear();
        }

        public void StopSpawning()
        {
            if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        }
    }
}