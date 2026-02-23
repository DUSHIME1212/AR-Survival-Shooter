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

            if (tapped && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }

        private void Shoot()
        {
            AudioManager.Instance.PlayPlayerShoot();
            GameObject bullet = ObjectPoolManager.Instance.GetPlayerBullet(firePoint.position, firePoint.rotation);
            // Bullet's OnEnable handles velocity application
        }
    }
}