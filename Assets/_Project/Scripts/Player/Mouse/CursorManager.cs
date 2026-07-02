using UnityEngine;

public enum CursorState
{
    Gameplay,
    UI
}

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetState(CursorState.Gameplay);
    }

    public void SetState(CursorState state)
    {
        switch (state)
        {
            case CursorState.Gameplay:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case CursorState.UI:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;

            default:
                Debug.LogWarning($"Unknown Cursor State : {state}");
                break;
        }
    }
}