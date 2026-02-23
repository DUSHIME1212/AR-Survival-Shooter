// Scripts/Data/EnemyData.cs
using UnityEngine;

namespace ARSurvivalShooter
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        public string enemyName;
        public int maxHealth;
        public float moveSpeed;
        public int attackDamage;
        public float attackRange;
        public float attackCooldown;
        public float shootingDistance; // only used by ShooterEnemy
        public int scoreValue;
        public Color normalColor = Color.white;
    }
}