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
            player.GetComponent<PlayerHealth>().TakeDamage(data.attackDamage);
            attackCooldownTimer = data.attackCooldown;
        }
    }
}