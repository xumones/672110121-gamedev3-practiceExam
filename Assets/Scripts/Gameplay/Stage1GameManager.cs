using UnityEngine;

namespace RavipoteP
{
    public class Stage1GameManager : MonoBehaviour
    {
        public static Stage1GameManager Instance { get; private set; }

        [Header("Game Settings")]
        [SerializeField] private float initialTime = 30f;
        [SerializeField] private int winThreshold = 10000;
        [SerializeField] private float fallKillY = -8f;

        [Header("References")]
        [SerializeField] private Transform playerTransform;

        public float RemainingTime { get; private set; }
        public int CurrentScore { get; private set; }
        public bool IsGameActive { get; private set; }

        public System.Action<float> OnTimeChanged;
        public System.Action<int> OnScoreChanged;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            RemainingTime = initialTime;
            CurrentScore = 0;
            IsGameActive = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetGame();
            }

            if (playerTransform == null)
            {
                var player = GameObject.FindWithTag("Player");
                if (player != null) playerTransform = player.transform;
            }

            OnTimeChanged?.Invoke(RemainingTime);
            OnScoreChanged?.Invoke(CurrentScore);
        }

        private void Update()
        {
            if (!IsGameActive) return;

            // Countdown timer
            RemainingTime -= Time.deltaTime;
            if (RemainingTime < 0f) RemainingTime = 0f;
            OnTimeChanged?.Invoke(RemainingTime);

            // Check fall off map
            if (playerTransform != null && playerTransform.position.y < fallKillY)
            {
                AudioManager.Instance?.PlayFallSound();
                TriggerGameOver("Fell off the map!");
                return;
            }

            // Check time expired
            if (RemainingTime <= 0f)
            {
                if (CurrentScore > winThreshold)
                {
                    TriggerWin();
                }
                else
                {
                    TriggerGameOver("Time ran out!");
                }
            }
        }

        public void AddScore(int amount)
        {
            if (!IsGameActive) return;

            CurrentScore = Mathf.Max(0, CurrentScore + amount);
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(amount);
            }
            OnScoreChanged?.Invoke(CurrentScore);
        }

        public void AddTime(float seconds)
        {
            if (!IsGameActive) return;

            RemainingTime = Mathf.Max(0f, RemainingTime + seconds);
            OnTimeChanged?.Invoke(RemainingTime);
        }

        public void TriggerWin()
        {
            if (!IsGameActive) return;
            IsGameActive = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.FinishGame(CurrentScore, true);
                GameManager.Instance.GoToWin();
            }
        }

        public void TriggerGameOver(string reason = "")
        {
            if (!IsGameActive) return;
            IsGameActive = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log($"Stage 1 Game Over: {reason} Final Score: {CurrentScore}");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.FinishGame(CurrentScore, false);
                GameManager.Instance.GoToGameOver();
            }
        }

        public void ReturnToMainMenu()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneNavigator.Instance?.GoToMainMenu();
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
