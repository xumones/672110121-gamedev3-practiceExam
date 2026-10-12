using UnityEngine;
using TMPro;

namespace RavipoteP
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (scoreText != null && GameManager.Instance != null)
            {
                scoreText.text = $"Score: {GameManager.Instance.Score}";
            }
        }

        public void OnTryAgainClicked()
        {
            GameManager.Instance?.ResetGame();
            SceneNavigator.Instance.ReplayLastStage();
        }

        public void OnMainMenuClicked()
        {
            GameManager.Instance?.ResetGame();
            SceneNavigator.Instance.GoToMainMenu();
        }
    }
}
