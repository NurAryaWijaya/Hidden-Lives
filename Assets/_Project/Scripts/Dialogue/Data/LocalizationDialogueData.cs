using UnityEngine;

namespace Game.Localization
{
    [CreateAssetMenu(
        fileName = "Localized Dialogue",
        menuName = "Hidden Live/Localization/Localized Dialogue")]
    public class LocalizedDialogueData : ScriptableObject
    {
        [Header("Dialogue")]

        [SerializeField]
        private Dialogue.DialogueData indonesian;

        [SerializeField]
        private Dialogue.DialogueData english;

        [SerializeField]
        private Dialogue.DialogueData japanese;

        public Dialogue.DialogueData GetDialogue()
        {
            switch (LocalizationManager.Instance.CurrentLanguage)
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