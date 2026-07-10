using UnityEngine;

namespace Game.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            musicSource.playOnAwake = false;
            sfxSource.playOnAwake = false;
        }

        #region Music

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null)
                return;

            if (musicSource.clip == clip &&
                musicSource.isPlaying)
                return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
            musicSource.clip = null;
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }

        public void ResumeMusic()
        {
            if (musicSource.clip != null)
                musicSource.UnPause();
        }

        #endregion

        #region SFX

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null)
                return;

            sfxSource.PlayOneShot(clip);
        }

        #endregion
    }
}