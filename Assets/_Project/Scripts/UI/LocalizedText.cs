using TMPro;
using UnityEngine;

namespace Game.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField]
        private bool useLocalizationKey = true;

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

        private void Refresh()
        {
            if (LocalizationManager.Instance == null)
                return;

            if (LocalizationManager.Instance.CurrentFont != null)
            {
                textComponent.font =
                    LocalizationManager.Instance.CurrentFont;
            }

            if (useLocalizationKey)
            {
                textComponent.text =
                    LocalizationManager.Instance.Get(key);
            }
        }

        private void Refresh(LocalizationLanguage language)
        {
            Refresh();
        }
    }
}