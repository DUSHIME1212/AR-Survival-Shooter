// Scripts/Player/ShootingController.cs
using UnityEngine;

namespace ARSurvivalShooter
{
    public class ShootingController : MonoBehaviour
    {
        [SerializeField] private Transform firePoint; // empty child at camera center
        [SerializeField] private float fireRate = 0.2f;

        private float nextFireTime;

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

            // Tap to shoot on mobile, left-click in Editor/Simulator
            bool tapped = (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                          || Input.GetMouseButtonDown(0);

            if (tapped)
            {
                Debug.Log($"[ShootingController] Tapped! Time.time: {Time.time}, nextFireTime: {nextFireTime}");
                if (Time.time >= nextFireTime)
                {
                    Shoot();
                    nextFireTime = Time.time + fireRate;
                }
            }
        }

        private void Shoot()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayPlayerShoot();

            if (ObjectPoolManager.Instance != null)
            {
                // Raycast from camera center to find target in 3D space.
                int projLayer = LayerMask.NameToLayer("Projectile");
                int playerLayer = LayerMask.NameToLayer("Player");
                
                // Mask: Start with everything, then EXCLUDE layers we don't want to hit (self/bullets)
                int mask = ~0; 
                if (projLayer != -1) mask &= ~(1 << projLayer);
                if (playerLayer != -1) mask &= ~(1 << playerLayer);

                // Note: We do NOT exclude "Character" anymore, as enemies might be on it.
                // We should only exclude the "Player" specifically.

                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
                {
                    Debug.Log($"[ShootingController] Raycast hit: {hit.collider.gameObject.name} on layer {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                    
                    // Apply damage if it's an enemy
                    EnemyBase enemy = hit.collider.GetComponentInParent<EnemyBase>();
                    if (enemy != null)
                    {
                        // You might want to get damage from a variable, but for now we'll use a default or 1
                        enemy.TakeDamage(1); 
                        Debug.Log("[ShootingController] Enemy hit! Applied 1 damage.");
                    }
                }
                else
                {
                    Debug.Log("[ShootingController] Raycast hit nothing.");
                }
            }
        }
    }
}