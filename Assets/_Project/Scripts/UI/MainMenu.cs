using Game.Audio;
using Game.Flow;
using Game.Localization;
using UnityEngine;

namespace Game.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField]
        private GameObject langguage;

        [SerializeField] private AudioClip audioClip;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SaveManager.Instance.Load();

            AudioManager.Instance.PlayMusic(audioClip);
        }

        public void OpenSettings()
        {
            langguage.SetActive(true);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;

            AudioManager.Instance.StopMusic();
#else
            Application.Quit();
#endif
        }

        public void ResetProgress()
        {
            SaveManager.Instance.Delete();
        }


        public void SetIndonesian() 
        { 
            LocalizationManager.Instance.SetLanguage(LocalizationLanguage.Indonesian);
            langguage.SetActive(false);
        }
        public void SetEnglish() 
        { 
            LocalizationManager.Instance.SetLanguage(LocalizationLanguage.English);
            langguage.SetActive(false);

        }
        public void SetJapanese() 
        { 
            LocalizationManager.Instance.SetLanguage(LocalizationLanguage.Japanese);
            langguage.SetActive(false);
        }
    }
}