using Game.Flow;
using System;
using UnityEngine;

namespace Game.Cutscene
{
    public class TimelineBranchController : MonoBehaviour
    {
        public event Action<bool> Selected;
        public static TimelineBranchController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Show(string trueText, string falseText)
        {
            GameStateManager.Instance.SetState(GameState.Branch);

            BranchManager.Instance.BranchSelected += OnBranchSelected;

            BranchManager.Instance.Show(trueText, falseText);
        }

        private void OnBranchSelected(bool result)
        {
            BranchManager.Instance.BranchSelected -= OnBranchSelected;

            GameStateManager.Instance.SetState(GameState.Cutscene);

            Selected?.Invoke(result);
        }
    }
}