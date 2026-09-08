using UnityEngine;

namespace Game.Localization
{
    [CreateAssetMenu(
        fileName = "Localized Branch",
        menuName = "Hidden Live/Localization/Localized Branch")]
    public class LocalizedBranchData : ScriptableObject
    {
        [SerializeField]
        private string trueKey;

        [SerializeField]
        private string falseKey;

        public string TrueText
        {
            get
            {
                if (LocalizationManager.Instance == null)
                    return trueKey;

                return LocalizationManager.Instance.Get(trueKey);
            }
        }

        public string FalseText
        {
            get
            {
                if (LocalizationManager.Instance == null)
                    return falseKey;

                return LocalizationManager.Instance.Get(falseKey);
            }
        }
    }
}