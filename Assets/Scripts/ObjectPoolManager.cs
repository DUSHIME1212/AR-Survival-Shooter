// Scripts/Pooling/ObjectPoolManager.cs
using UnityEngine;

namespace ARSurvivalShooter
{
    // Singleton that owns all pools and hands out bullets
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance { get; private set; }

        [SerializeField] private ObjectPool playerBulletPool;
        [SerializeField] private ObjectPool enemyBulletPool;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
        }

        public GameObject GetPlayerBullet(Vector3 pos, Quaternion rot) => playerBulletPool.Get(pos, rot);
        public void ReturnPlayerBullet(GameObject b) => playerBulletPool.Return(b);
        public GameObject GetEnemyBullet(Vector3 pos, Quaternion rot) => enemyBulletPool.Get(pos, rot);
        public void ReturnEnemyBullet(GameObject b) => enemyBulletPool.Return(b);
    }
}
