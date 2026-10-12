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

        [Header("SFX Clips")]
        [SerializeField] private AudioClip buttonClickSFX;
        [SerializeField] private AudioClip winSFX;
        [SerializeField] private AudioClip gameOverSFX;
        [SerializeField] private AudioClip jumpSFX;
        [SerializeField] private AudioClip fallSFX;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Ensure AudioSources exist
            if (bgmSource == null)
            {
                bgmSource = gameObject.AddComponent<AudioSource>();
                bgmSource.playOnAwake = false;
                bgmSource.loop = true;
            }
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
                sfxSource.loop = false;
            }

            // Load saved volumes
            SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 1f));
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Start()
        {
            HookAllSceneButtons();
            if (bgmSource != null && !bgmSource.isPlaying && bgmClips != null && bgmClips.Length > 0)
            {
                PlayBGMByIndex(0);
            }
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            HookAllSceneButtons();

            if (scene.name == "Win")
            {
                PlayWinSound();
            }
            else if (scene.name == "GameOver")
            {
                PlayGameOverSound();
            }
        }

        public void HookAllSceneButtons()
        {
            var buttons = FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None);
            foreach (var btn in buttons)
            {
                btn.onClick.RemoveListener(PlayButtonClick);
                btn.onClick.AddListener(PlayButtonClick);
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

        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            if (sfxSource == null || clip == null) return;
            sfxSource.PlayOneShot(clip, volumeScale);
        }

        public void PlayButtonClick()
        {
            if (buttonClickSFX != null)
                PlaySFX(buttonClickSFX, 0.7f);
        }

        public void PlayWinSound()
        {
            if (winSFX != null)
                PlaySFX(winSFX, 0.85f);
        }

        public void PlayGameOverSound()
        {
            if (gameOverSFX != null)
                PlaySFX(gameOverSFX, 0.85f);
        }

        public void PlayJumpSound(float volumeScale = 0.55f)
        {
            if (jumpSFX != null)
                PlaySFX(jumpSFX, volumeScale);
        }

        public void PlayFallSound()
        {
            if (fallSFX != null)
                PlaySFX(fallSFX);
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
