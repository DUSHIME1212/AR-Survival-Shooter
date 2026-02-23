// Scripts/Enemies/ShooterEnemy.cs
using UnityEngine;

namespace ARSurvivalShooter
{
    public class ShooterEnemy : EnemyBase
    {
        [SerializeField] private Transform firePoint;

        protected override void BehaviorUpdate()
        {
            float dist = Vector3.Distance(transform.position, player.position);
            transform.LookAt(player);

            // Move only if outside preferred shooting range
            if (dist > data.shootingDistance)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, player.position, data.moveSpeed * Time.deltaTime);
            }

            // Shoot when in range and cooldown expired
            if (dist <= data.shootingDistance && attackCooldownTimer <= 0f)
                Attack();
        }

        protected override void Attack()
        {
            AudioManager.Instance.PlayEnemyShoot();
            GameObject bullet = ObjectPoolManager.Instance.GetEnemyBullet(firePoint.position, firePoint.rotation);
            attackCooldownTimer = data.attackCooldown;
        }
    }
}