using System;

namespace Game.Localization
{
    [Serializable]
    public class LocalizationEntry
    {
        public string Key;

        [UnityEngine.TextArea]
        public string Value;
    }
}