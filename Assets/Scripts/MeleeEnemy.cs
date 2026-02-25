// Scripts/Enemies/MeleeEnemy.cs
using UnityEngine;

namespace ARSurvivalShooter
{
    public class MeleeEnemy : EnemyBase
    {
        protected override void BehaviorUpdate()
        {
            float dist = Vector3.Distance(transform.position, player.position);

            // Always move toward player
            transform.position = Vector3.MoveTowards(
                transform.position, player.position, data.moveSpeed * Time.deltaTime);
            transform.LookAt(player);

            // Attack when close enough and cooldown expired
            if (dist <= data.attackRange && attackCooldownTimer <= 0f)
                Attack();
        }

        protected override void Attack()
        {
            AudioManager.Instance.PlayMeleeAttack();
            var health = player.GetComponentInParent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(data.attackDamage);
                Debug.Log($"[MeleeEnemy] Attacked player! Dealing {data.attackDamage} damage.");
            }
            else
            {
                Debug.LogWarning($"[MeleeEnemy] Could not find PlayerHealth on {player.name} or parents!");
            }
            attackCooldownTimer = data.attackCooldown;
        }
    }
}