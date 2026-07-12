using TMPro;
using UnityEngine;

namespace Game.Localization
{
    [CreateAssetMenu(
        fileName = "Localization Font Database",
        menuName = "Hidden Live/Localization/Localization Font Database")]
    public class LocalizationFontDatabase : ScriptableObject
    {
        [Header("Fonts")]

        [SerializeField]
        private TMP_FontAsset indonesian;

        [SerializeField]
        private TMP_FontAsset english;

        [SerializeField]
        private TMP_FontAsset japanese;

        public TMP_FontAsset GetFont(LocalizationLanguage language)
        {
            switch (language)
            {
                case LocalizationLanguage.English:
                    return english != null
                        ? english
                        : indonesian;

                case LocalizationLanguage.Japanese:
                    return japanese != null
                        ? japanese
                        : indonesian;

                case LocalizationLanguage.Indonesian:
                default:
                    return indonesian;
            }
        }
    }
}