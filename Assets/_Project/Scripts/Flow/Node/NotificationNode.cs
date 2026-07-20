using Game.Localization;
using Game.UI;
using UnityEngine;

namespace Game.Flow
{
    public class NotificationNode : FlowNode
    {
        [Header("Notification")]

        [SerializeField]
        private string localizationKey;

        [SerializeField]
        private float duration = 5f;

        public override void Enter()
        {
            string text =
                LocalizationManager.Instance.Get(localizationKey);

            NotificationManager.Instance.Show(
                text,
                duration);

            Complete(nextNode);
        }
    }
}