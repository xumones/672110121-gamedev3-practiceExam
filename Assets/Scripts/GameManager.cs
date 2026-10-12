using UnityEngine;

namespace RavipoteP
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameManager>();
                    if (_instance == null)
                    {
                        var go = new GameObject("GameManager");
                        _instance = go.AddComponent<GameManager>();
                    }
                }
                return _instance;
            }
        }

        public int Score { get; private set; } = 0;
        public bool IsWin { get; private set; } = false;
        public const int WinThreshold = 10000;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
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
