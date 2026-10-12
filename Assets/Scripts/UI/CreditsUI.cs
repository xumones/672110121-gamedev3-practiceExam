using UnityEngine;

namespace RavipoteP
{
    public class CreditsUI : MonoBehaviour
    {
        public void OnBackClicked()
        {
            SceneNavigator.Instance.LoadScene("MainMenu");
        }
    }
}
