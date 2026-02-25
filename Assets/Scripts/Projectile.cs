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

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            
            // Auto-assign tag and layer
            gameObject.tag = "Projectile";
            int layer = LayerMask.NameToLayer("Projectile");
            if (layer != -1) gameObject.layer = layer;
        }

        private void OnEnable()
        {
            // Reset state when pulled from pool
            timer = 0f;
            
            // Configure Rigidbody for straight movement
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            
            // Ensure no damping so bullets don't slow down
            rb.linearDamping = 0f;
            rb.angularDamping = 0f;
            
            // Set velocity in the forward direction
            rb.linearVelocity = transform.forward * speed;
            rb.angularVelocity = Vector3.zero;

            // Optional: Ensure bullet is on a specific layer if needed
            gameObject.layer = LayerMask.NameToLayer("Projectile"); 
            Debug.Log($"[Projectile] Bullet spawned/reset. Direction: {transform.forward}");
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= lifetime) ReturnToPool();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isPlayerBullet)
            {
                var enemy = other.GetComponentInParent<EnemyBase>();
                if (enemy != null)
                {
                    Debug.Log($"[Projectile] Hit enemy: {other.gameObject.name}! Dealing {damage} damage.");
                    enemy.TakeDamage(damage);
                    ReturnToPool();
                }
                else
                {
                    Debug.Log($"[Projectile] Player bullet hit non-enemy: {other.gameObject.name} on layer {LayerMask.LayerToName(other.gameObject.layer)}");
                }
            }
            else
            {
                var playerHealth = other.GetComponentInParent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log($"[Projectile] Enemy bullet hit player! Dealing {damage} damage.");
                    playerHealth.TakeDamage(damage);
                    ReturnToPool();
                }
                else
                {
                    Debug.Log($"[Projectile] Enemy bullet hit non-player: {other.gameObject.name} on layer {LayerMask.LayerToName(other.gameObject.layer)}");
                }
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