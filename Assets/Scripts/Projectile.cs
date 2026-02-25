// Scripts/Player/Projectile.cs
using UnityEngine;

namespace ARSurvivalShooter
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 20f;
        [SerializeField] private float lifetime = 3f;
        [SerializeField] private int damage = 1;
        [SerializeField] private bool isPlayerBullet = true;

        private Rigidbody rb;
        private float timer;

        private void Awake() => rb = GetComponent<Rigidbody>();

        private void OnEnable()
        {
            // Reset state when pulled from pool
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            timer = 0f;
            // Removed velocity application here to use transform movement instead
        }

        private void Update()
        {
            // Move forward every frame
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            timer += Time.deltaTime;
            if (timer >= lifetime) ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isPlayerBullet && other.TryGetComponent<EnemyBase>(out var enemy))
            {
                enemy.TakeDamage(damage);
                ReturnToPool();
            }
            else if (!isPlayerBullet && other.TryGetComponent<PlayerHealth>(out var player))
            {
                player.TakeDamage(damage);
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            if (isPlayerBullet)
                ObjectPoolManager.Instance.ReturnPlayerBullet(gameObject);
            else
                ObjectPoolManager.Instance.ReturnEnemyBullet(gameObject);
        }
    }
}