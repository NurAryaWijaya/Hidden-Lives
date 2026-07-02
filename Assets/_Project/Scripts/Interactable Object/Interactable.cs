using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField]
    private float interactionDistance = 2f;

    public float InteractionDistance => interactionDistance;

    public InteractionBillboard Billboard { get; private set; }

    protected virtual void Awake()
    {
        Billboard = GetComponentInChildren<InteractionBillboard>(true);

        if (Billboard == null)
        {
            Debug.LogWarning($"{name} tidak memiliki InteractionBillboard.");
        }
    }

    #region Billboard

    public void ShowDot()
    {
        Billboard?.ShowDot();
    }

    public void ShowKey()
    {
        Billboard?.ShowKey();
    }

    public void Hide()
    {
        Billboard?.Hide();
    }
    #endregion

    public virtual bool CanInteract(PlayerInteractor player)
    {
        return true;
    }

    public abstract void Interact(PlayerInteractor player);

}