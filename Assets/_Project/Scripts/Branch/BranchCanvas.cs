using Game.Localization;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Flow
{
    public class BranchCanvas : MonoBehaviour
    {
        [SerializeField]
        private GameObject root;

        [SerializeField]
        private Button trueButton;

        [SerializeField]
        private Button falseButton;

        [SerializeField]
        private TMPro.TextMeshProUGUI trueLabel;

        [SerializeField]
        private TMPro.TextMeshProUGUI falseLabel;

        private void Awake()
        {
            root.SetActive(false);
        }

        public void Show(
            string trueText,
            string falseText,
            Action onTrue,
            Action onFalse)
        {
            root.SetActive(true);

            trueLabel.font = LocalizationManager.Instance.CurrentFont;
            falseLabel.font = LocalizationManager.Instance.CurrentFont;

            trueLabel.text = trueText;
            falseLabel.text = falseText;

            trueButton.onClick.RemoveAllListeners();
            falseButton.onClick.RemoveAllListeners();

            trueButton.onClick.AddListener(() => onTrue?.Invoke());
            falseButton.onClick.AddListener(() => onFalse?.Invoke());
        }

        public void Hide()
        {
            root.SetActive(false);

            trueButton.onClick.RemoveAllListeners();
            falseButton.onClick.RemoveAllListeners();
        }
    }
}