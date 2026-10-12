using UnityEngine;

namespace RavipoteP
{
    public class StageSelectionUI : MonoBehaviour
    {
        public void OnStage1Clicked()
        {
            GameManager.Instance?.ResetGame();
            SceneNavigator.Instance.LoadScene("Stage1_Capsule");
        }

        public void OnStage2Clicked()
        {
            GameManager.Instance?.ResetGame();
            SceneNavigator.Instance.LoadScene("Stage2_Humanoid");
        }

        public void OnBackClicked()
        {
            SceneNavigator.Instance.LoadScene("MainMenu");
        }
    }
}
