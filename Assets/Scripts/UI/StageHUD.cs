using UnityEngine;
using TMPro;

namespace RavipoteP
{
    public class StageHUD : MonoBehaviour
    {
        [Header("UI Text References")]
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text goalText;

        private void Start()
        {
            var stageMgr = Stage1GameManager.Instance;
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
        }

        private void OnDestroy()
        {
            var stageMgr = Stage1GameManager.Instance;
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
                timerText.color = new Color(1f, 0.25f, 0.25f); // Red alert
            }
            else if (time <= 10f)
            {
                timerText.color = new Color(1f, 0.85f, 0.2f); // Yellow warning
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

        public void OnMenuClicked()
        {
            SceneNavigator.Instance?.GoToMainMenu();
        }
    }
}
