using UnityEngine;

namespace RavipoteP
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<AudioManager>();
                    if (_instance == null)
                    {
                        var go = new GameObject("AudioManager");
                        _instance = go.AddComponent<AudioManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("BGM Clips")]
        [SerializeField] private AudioClip[] bgmClips;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Load saved volumes
            SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 1f));
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
        }

        private void Start()
        {
            if (bgmSource != null && !bgmSource.isPlaying && bgmClips != null && bgmClips.Length > 0)
            {
                PlayBGMByIndex(0);
            }
        }

        public void PlayBGM(AudioClip clip)
        {
            if (bgmSource == null) return;
            if (bgmSource.clip == clip && bgmSource.isPlaying) return;
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        public void PlayBGMByIndex(int index)
        {
            if (bgmClips == null || index < 0 || index >= bgmClips.Length) return;
            PlayBGM(bgmClips[index]);
        }

        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource == null || clip == null) return;
            sfxSource.PlayOneShot(clip);
        }

        public void StopBGM()
        {
            bgmSource?.Stop();
        }

        public void SetBGMVolume(float volume)
        {
            if (bgmSource != null)
                bgmSource.volume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            if (sfxSource != null)
                sfxSource.volume = volume;
        }
    }
}
