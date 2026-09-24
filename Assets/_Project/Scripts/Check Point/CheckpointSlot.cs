using Game.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Flow
{
    public class CheckpointSlot : MonoBehaviour
    {
        [SerializeField]
        private string checkpointId;

        [Header("UI")]
        [SerializeField]
        private Button loadButton;

        public string CheckpointId => checkpointId;

        public void Refresh()
        {
            bool unlocked =
                CheckpointManager.Instance.IsUnlocked(checkpointId);

            gameObject.SetActive(unlocked);
        }

        public void LoadCheckpoint()
        {
            AudioManager.Instance.StopMusic();
            TransitionManager.Instance.PlayTransition();
            CheckpointManager.Instance.Load(checkpointId);
        }
    }
}