using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Flow
{
    public class CheckpointCanvas : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private Transform content;

        [SerializeField]
        private CheckpointSlot[] slots;

        private bool isOpen;
        public static CheckpointCanvas Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            panel.SetActive(false);

            slots = content.GetComponentsInChildren<CheckpointSlot>(true);
        }

        private void Update()
        {
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                if (panel.activeSelf)
                {
                    Hide();
                }
                else
                    Show();
            }
        }

        public void Show()
        {
            RefreshSlots();

            panel.SetActive(true);

            GameStateManager.Instance.SetState(GameState.Pause);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Hide()
        {
            panel.SetActive(false);

            GameStateManager.Instance.SetState(GameState.Exploration);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void RefreshSlots()
        {
            foreach (CheckpointSlot slot in slots)
            {
                slot.Refresh();
            }
        }
    }
}