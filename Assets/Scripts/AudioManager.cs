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
            // Use ReferenceEquals to detect a truly non-null C# reference.
            // Unity's overloaded != null returns false for *destroyed* objects,
            // so a plain "Instance != null" check can miss a stale destroyed ref.
            if (!ReferenceEquals(Instance, null) && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        private void OnDestroy()
        {
            // Clear the static reference when this object is destroyed
            // so callers get a proper null instead of a stale destroyed ref.
            if (ReferenceEquals(Instance, this))
                Instance = null;
        }

        private void PlayClip(AudioClip clip)
        {
            // Guard: this instance may have been destroyed between frames
            if (clip == null || this == null) return;

            if (sfxSource == null)
            {
                sfxSource = GetComponent<AudioSource>();
                if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();
            }

            if (sfxSource != null)
                sfxSource.PlayOneShot(clip);
        }

        // One method per sound event — clean API
        public void PlayPlayerShoot() => PlayClip(playerShootClip);
        public void PlayPlayerDeath() => PlayClip(playerDeathClip);
        public void PlayEnemySpawn() => PlayClip(enemySpawnClip);
        public void PlayEnemyShoot() => PlayClip(enemyShootClip);
        public void PlayMeleeAttack() => PlayClip(meleeAttackClip);
    }
}
