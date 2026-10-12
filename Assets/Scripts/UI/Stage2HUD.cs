using UnityEngine;
using TMPro;

namespace RavipoteP
{
    public class Stage2HUD : MonoBehaviour
    {
        public static Stage2HUD Instance { get; private set; }

        [Header("UI Text References")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text goalText;

        [Header("Interaction Prompt")]
        [SerializeField] private GameObject promptContainer;
        [SerializeField] private TMP_Text promptText;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            var stageMgr = Stage2GameManager.Instance;
            if (stageMgr != null)
            {
                stageMgr.OnTimeChanged += UpdateTimer;
                stageMgr.OnScoreChanged += UpdateScore;
                UpdateTimer(stageMgr.RemainingTime);
                UpdateScore(stageMgr.CurrentScore);
            }

            if (goalText != null)
            {
                goalText.text = "Goal: > 10,000 pts";
            }

            HideInteractionPrompt();
        }

        private void OnDestroy()
        {
            var stageMgr = Stage2GameManager.Instance;
            if (stageMgr != null)
            {
                stageMgr.OnTimeChanged -= UpdateTimer;
                stageMgr.OnScoreChanged -= UpdateScore;
            }
        }

        private void UpdateTimer(float time)
        {
            if (timerText == null) return;

            timerText.text = $"Time: {time:F1}s";
            if (time <= 5f)
            {
                timerText.color = new Color(1f, 0.25f, 0.25f);
            }
            else if (time <= 10f)
            {
                timerText.color = new Color(1f, 0.85f, 0.2f);
            }
            else
            {
                timerText.color = Color.white;
            }
        }

        private void UpdateScore(int score)
        {
            if (scoreText == null) return;
            scoreText.text = $"Score: {score:N0}";
        }

        public void ShowInteractionPrompt(string text)
        {
            if (promptContainer != null) promptContainer.SetActive(true);
            if (promptText != null) promptText.text = text;
        }

        public void HideInteractionPrompt()
        {
            if (promptContainer != null) promptContainer.SetActive(false);
        }

        public void OnMenuClicked()
        {
            SceneNavigator.Instance?.GoToMainMenu();
        }
    }
}
