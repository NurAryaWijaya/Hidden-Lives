using UnityEngine;

public enum InteractionState
{
    Hidden,
    Dot,
    Key
}

public class InteractionBillboard : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject dotIcon;
    [SerializeField] private GameObject keyIcon;

    private InteractionState currentState = InteractionState.Hidden;
    private Camera targetCamera;

    private void Awake()
    {
        currentState = (InteractionState)(-1); // atau gunakan field bool initialized
        Hide();
    }

    private void Start()
    {
        targetCamera = Camera.main;

        if (targetCamera == null)
        {
            Debug.LogError("Main Camera tidak ditemukan.");
        }
    }

    private void LateUpdate()
    {
        if (targetCamera == null)
            return;

        transform.rotation = Quaternion.LookRotation(
            transform.position - targetCamera.transform.position);
    }

    public void ShowDot()
    {
        if (currentState == InteractionState.Dot)
            return;

        if (!GameStateManager.Instance.IsState(GameState.Exploration))
            return;

        currentState = InteractionState.Dot;

        dotIcon.SetActive(true);
        keyIcon.SetActive(false);
    }

    public void ShowKey()
    {
        if (currentState == InteractionState.Key)
            return;

        if (!GameStateManager.Instance.IsState(GameState.Exploration))
            return;

        currentState = InteractionState.Key;

        dotIcon.SetActive(false);
        keyIcon.SetActive(true);
    }

    public void Hide()
    {
        if (currentState == InteractionState.Hidden)
            return;

        currentState = InteractionState.Hidden;

        dotIcon.SetActive(false);
        keyIcon.SetActive(false);
    }

}