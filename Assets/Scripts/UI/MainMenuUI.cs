using UnityEngine;

namespace RavipoteP
{
    public class MainMenuUI : MonoBehaviour
    {
        public void OnStartClicked()
        {
            SceneNavigator.Instance.LoadScene("StageSelection");
        }

        public void OnOptionsClicked()
        {
            SceneNavigator.Instance.LoadScene("Options");
        }

        public void OnCreditsClicked()
        {
            SceneNavigator.Instance.LoadScene("Credits");
        }

        public void OnQuitClicked()
        {
            SceneNavigator.Instance.QuitGame();
        }
    }
}
