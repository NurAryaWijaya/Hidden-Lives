using System.Collections.Generic;
using UnityEngine;

namespace Game.Localization
{
    [CreateAssetMenu(
        fileName = "Localization Database",
        menuName = "Hidden Live/Localization/Localization Database")]
    public class LocalizationDatabase : ScriptableObject
    {
        [SerializeField]
        private List<LocalizationEntry> entries = new();

        public IReadOnlyList<LocalizationEntry> Entries => entries;

        public string Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return string.Empty;

            foreach (LocalizationEntry entry in entries)
            {
                if (entry.Key == key)
                    return entry.Value;
            }

            Debug.LogWarning(
                $"Localization key '{key}' tidak ditemukan di '{name}'.");

            return key;
        }
    }
}