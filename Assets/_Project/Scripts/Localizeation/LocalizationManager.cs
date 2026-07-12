using Game.Localization;
using System;
using UnityEngine;
using TMPro;

namespace Game.Localization
{
    public enum LocalizationLanguage
    {
        Indonesian,
        English,
        Japanese
    }

    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        public event Action<LocalizationLanguage> LanguageChanged;

        [Header("Default")]
        [SerializeField]
        private LocalizationLanguage currentLanguage =
            LocalizationLanguage.Indonesian;

        [Header("Font")]
        [SerializeField]
        private LocalizationFontDatabase fontDatabase;

        [Header("Database")]
        [SerializeField]
        private LocalizationDatabase indonesian;

        [SerializeField]
        private LocalizationDatabase english;

        [SerializeField]
        private LocalizationDatabase japanese;

        public TMP_FontAsset CurrentFont
        {
            get
            {
                if (fontDatabase == null)
                    return null;

                return fontDatabase.GetFont(CurrentLanguage);
            }
        }

        public LocalizationLanguage CurrentLanguage => currentLanguage;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void SetLanguage(LocalizationLanguage language)
        {
            if (currentLanguage == language)
                return;

            currentLanguage = language;

            RefreshLanguage();      
        }

        public string Get(string key)
        {
            LocalizationDatabase database = GetCurrentDatabase();

            if (database == null)
            {
                Debug.LogError("Localization Database belum diisi.");
                return key;
            }

            return database.Get(key);
        }

        private LocalizationDatabase GetCurrentDatabase()
        {
            switch (currentLanguage)
            {
                case LocalizationLanguage.English:
                    return english;

                case LocalizationLanguage.Japanese:
                    return japanese;

                case LocalizationLanguage.Indonesian:
                default:
                    return indonesian;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
                return;

            RefreshLanguage();
        }
#endif

        private void RefreshLanguage()
        {
            LanguageChanged?.Invoke(currentLanguage);
        }
    }
}

// Cara memanggilnya misalnya :SetLanguage(LocalizationLanguage.Indonesian);