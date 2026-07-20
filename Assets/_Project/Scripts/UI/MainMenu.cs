using Game.Flow;
using UnityEngine;

namespace Game.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField]
        private GameObject settingPanel;

        private void Start()
        {
            SaveManager.Instance.Load();
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

        public void ResetProgress()
        {
            SaveManager.Instance.Delete();
        }
    }
}