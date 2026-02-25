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
                Vector3 targetPoint;
                
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, mask))
                {
                    targetPoint = hit.point;
                    Debug.Log($"[ShootingController] Raycast hit: {hit.collider.gameObject.name} on layer {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                }
                else
                {
                    targetPoint = ray.GetPoint(50f);
                    Debug.Log("[ShootingController] Raycast hit nothing, targeting point 50m ahead.");
                }

                if (firePoint == null)
                {
                    Debug.LogError("[ShootingController] FirePoint is NULL! Please assign it in the Inspector.");
                    return;
                }

                // Calculate direction to target
                Vector3 direction = (targetPoint - firePoint.position).normalized;
                Quaternion bulletRotation = Quaternion.LookRotation(direction);

                if (ObjectPoolManager.Instance == null)
                {
                    Debug.LogError("[ShootingController] ObjectPoolManager instance is NULL!");
                    return;
                }

                GameObject bullet = ObjectPoolManager.Instance.GetPlayerBullet(firePoint.position, bulletRotation);
                if (bullet != null)
                {
                    Debug.Log($"[ShootingController] SUCCESSFULLY fired bullet: {bullet.name} from {firePoint.position} towards {targetPoint}");
                }
                else
                {
                    Debug.LogError("[ShootingController] FAILED to get bullet from pool! Check if the prefab is assigned in the ObjectPoolManager.");
                }
            }
        }
    }
}