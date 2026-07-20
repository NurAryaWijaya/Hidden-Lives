using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager Instance { get; private set; }

        [Header("UI")]

        [SerializeField]
        private RectTransform panel;

        [SerializeField]
        private TMP_Text messageText;

        [Header("Animation")]

        [SerializeField]
        private Vector2 hiddenPosition = new(-600f, 0f);

        [SerializeField]
        private Vector2 shownPosition = new(30f, 0f);

        [SerializeField]
        private float moveDuration = 1f;

        private Coroutine routine;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            panel.anchoredPosition = hiddenPosition;
            panel.gameObject.SetActive(false);
        }

        public void Show(
            string message,
            float duration)
        {
            if (routine != null)
                StopCoroutine(routine);

            routine = StartCoroutine(
                ShowRoutine(
                    message,
                    duration));
        }

        private IEnumerator ShowRoutine(
            string message,
            float duration)
        {
            messageText.text = message;

            panel.gameObject.SetActive(true);

            yield return Move(
                hiddenPosition,
                shownPosition);

            yield return new WaitForSeconds(duration);

            yield return Move(
                shownPosition,
                hiddenPosition);

            panel.gameObject.SetActive(false);

            routine = null;
        }

        private IEnumerator Move(
            Vector2 from,
            Vector2 to)
        {
            float timer = 0f;

            while (timer < moveDuration)
            {
                timer += Time.deltaTime;

                panel.anchoredPosition =
                    Vector2.Lerp(
                        from,
                        to,
                        timer / moveDuration);

                yield return null;
            }

            panel.anchoredPosition = to;
        }
    }
}