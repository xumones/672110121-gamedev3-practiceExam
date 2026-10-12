using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RavipoteP
{
    public class SceneNavigator : MonoBehaviour
    {
        private static SceneNavigator _instance;
        public static SceneNavigator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<SceneNavigator>();
                    if (_instance == null)
                    {
                        var go = new GameObject("SceneNavigator");
                        _instance = go.AddComponent<SceneNavigator>();
                    }
                }
                return _instance;
            }
        }

        private readonly Stack<string> sceneHistory = new Stack<string>();
        public string LastPlayedStage { get; set; } = "Stage1_Capsule";

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

        public void LoadScene(string sceneName)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene != sceneName)
            {
                sceneHistory.Push(currentScene);
            }

            if (sceneName.StartsWith("Stage"))
            {
                LastPlayedStage = sceneName;
            }

            SceneManager.LoadScene(sceneName);
        }

        public void LoadPreviousScene()
        {
            if (sceneHistory.Count > 0)
            {
                string previousScene = sceneHistory.Pop();
                SceneManager.LoadScene(previousScene);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

        public void ReplayLastStage()
        {
            if (!string.IsNullOrEmpty(LastPlayedStage))
            {
                SceneManager.LoadScene(LastPlayedStage);
            }
            else
            {
                SceneManager.LoadScene("Stage1_Capsule");
            }
        }

        public void GoToMainMenu()
        {
            sceneHistory.Clear();
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
