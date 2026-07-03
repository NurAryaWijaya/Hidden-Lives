using UnityEngine;
using Game.Dialogue;

namespace Game.Dialogue
{
    public class DialogueInteractable : Interactable
    {
        [Header("Dialogue")]
        [SerializeField] private DialogueData dialogueData;
        [SerializeField] private Transform headFocusPoint;

        private int interactableLayer;
        private int heldLayer;

        protected override void Awake()
        {
            base.Awake();

            interactableLayer = LayerMask.NameToLayer("Interactable");
            heldLayer = LayerMask.NameToLayer("HeldObject");
        }

        public override void Interact(PlayerInteractor player)
        {
            if (dialogueData == null)
            {
                Debug.LogWarning($"{name} tidak memiliki DialogueData.");
                return;
            }

            SetLayerRecursively(transform, heldLayer);

            DialogueManager.Instance.StartDialogue(dialogueData, headFocusPoint);
        }

        private void SetLayerRecursively(Transform root, int layer)
        {
            root.gameObject.layer = layer;

            foreach (Transform child in root)
            {
                SetLayerRecursively(child, layer);
            }
        }
    }
}