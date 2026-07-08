using System;
using UnityEngine;

namespace Game.Flow
{
    public class BranchManager : MonoBehaviour
    {
        public static BranchManager Instance { get; private set; }

        [SerializeField]
        private BranchCanvas branchCanvas;

        public event Action<bool> BranchSelected;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            branchCanvas.Hide();
        }

        public void Show(string trueText, string falseText)
        {
            branchCanvas.Show(
                trueText,
                falseText,
                SelectTrue,
                SelectFalse);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Hide()
        {
            branchCanvas.Hide();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void SelectTrue()
        {
            Hide();

            BranchSelected?.Invoke(true);
        }

        private void SelectFalse()
        {
            Hide();

            BranchSelected?.Invoke(false);
        }
    }
}