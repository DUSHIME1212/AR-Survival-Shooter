// Scripts/Audio/AudioManager.cs
using UnityEngine;

namespace ARSurvivalShooter
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Player SFX")]
        [SerializeField] private AudioClip playerShootClip;
        [SerializeField] private AudioClip playerDeathClip;

        [Header("Enemy SFX")]
        [SerializeField] private AudioClip enemySpawnClip;
        [SerializeField] private AudioClip enemyShootClip;
        [SerializeField] private AudioClip meleeAttackClip;

        private AudioSource sfxSource;

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists across scenes

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        private void PlayClip(AudioClip clip)
        {
            if (clip != null) sfxSource.PlayOneShot(clip);
        }

        // One method per sound event — clean API
        public void PlayPlayerShoot() => PlayClip(playerShootClip);
        public void PlayPlayerDeath() => PlayClip(playerDeathClip);
        public void PlayEnemySpawn() => PlayClip(enemySpawnClip);
        public void PlayEnemyShoot() => PlayClip(enemyShootClip);
        public void PlayMeleeAttack() => PlayClip(meleeAttackClip);
    }
}