using UnityEngine;

namespace RavipoteP
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public int Score { get; private set; } = 0;
        public bool IsWin { get; private set; } = false;
        public const int WinThreshold = 10000;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void ResetGame()
        {
            Score = 0;
            IsWin = false;
        }

        public void AddScore(int amount)
        {
            Score += amount;
        }

        public void SetWin(bool win)
        {
            IsWin = win;
        }

        public void FinishGame(int finalScore, bool win)
        {
            Score = finalScore;
            IsWin = win;
        }

        public void GoToWin()
        {
            IsWin = true;
            if (SceneNavigator.Instance != null)
                SceneNavigator.Instance.LoadScene("Win");
        }

        public void GoToGameOver()
        {
            IsWin = false;
            if (SceneNavigator.Instance != null)
                SceneNavigator.Instance.LoadScene("GameOver");
        }
    }
}
