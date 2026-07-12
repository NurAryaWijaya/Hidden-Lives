using UnityEngine;

namespace Game.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Panels")]

        [SerializeField]
        private GameObject loadPanel;

        [SerializeField]
        private GameObject settingPanel;

        public void OpenLoad()
        {
            if (loadPanel != null)
                loadPanel.SetActive(true);
        }

        public void OpenSettings()
        {
            if (settingPanel != null)
                settingPanel.SetActive(true);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}