using TMPro;
using UnityEngine;

namespace Game.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class ChatLocalizedText : MonoBehaviour
    {
        [Header("Database")]
        [SerializeField]
        private LocalizationDatabase indonesian;

        [SerializeField]
        private LocalizationDatabase english;

        [SerializeField]
        private LocalizationDatabase japanese;

        [Header("Localization")]
        [SerializeField]
        private string key;

        private TMP_Text textComponent;

        private void Awake()
        {
            textComponent = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            Refresh();

            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.LanguageChanged += Refresh;
            }
        }

        private void OnDestroy()
        {
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.LanguageChanged -= Refresh;
            }
        }

        public void SetKey(string localizationKey)
        {
            key = localizationKey;
            Refresh();
        }

        private void Refresh()
        {
            if (LocalizationManager.Instance == null)
                return;

            if (LocalizationManager.Instance.CurrentFont != null)
            {
                textComponent.font =
                    LocalizationManager.Instance.CurrentFont;
            }

            textComponent.text =
                LocalizationManager.Instance.Get(
                    key,
                    indonesian,
                    english,
                    japanese);
        }

        private void Refresh(LocalizationLanguage language)
        {
            Refresh();
        }
    }
}