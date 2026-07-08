using System.Collections.Generic;
using UnityEngine;
using Game.Dialogue;

[RequireComponent(typeof(PlayerCarry))]
public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Interaction")]
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private float detectDistance = 4f;

    private readonly Collider[] overlapResults = new Collider[32];

    // Objek dalam radius detect
    private readonly HashSet<Interactable> nearbyObjects = new();

    // Objek yang ditemukan frame ini
    private readonly HashSet<Interactable> currentFrameObjects = new();

    public PlayerCarry PlayerCarry { get; private set; }

    // Objek yang sedang dilihat
    private Interactable lookTarget;

    // Objek yang bisa ditekan E
    private Interactable interactTarget;

    // Objek yang dilihat frame sebelumnya
    private Interactable previousLookTarget;

    private void Awake()
    {
        PlayerCarry = GetComponent<PlayerCarry>();
    }

    private void Update()
    {
        DetectNearby();
        DetectLookTarget();
        HandleInput();
    }

    #region Detect Nearby

    private void DetectNearby()
    {
        currentFrameObjects.Clear();

        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            detectDistance,
            overlapResults,
            interactMask);

        for (int i = 0; i < count; i++)
        {
            Interactable interactable =
                overlapResults[i].GetComponentInParent<Interactable>();

            if (interactable == null)
                continue;

            if (!currentFrameObjects.Add(interactable))
                continue;

            if (!nearbyObjects.Contains(interactable))
            {
                nearbyObjects.Add(interactable);
            }

            if (interactable.CanInteract(this))
                interactable.ShowDot();
            else
                interactable.Hide();
        }

        nearbyObjects.RemoveWhere(interactable =>
        {
            if (currentFrameObjects.Contains(interactable))
                return false;

            interactable.Hide();
            return true;
        });
    }

    #endregion

    #region Detect Look

    private void DetectLookTarget()
    {
        interactTarget = null;
        lookTarget = null;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        // Raycast tidak dibatasi detectDistance lagi
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, interactMask))
        {
            RestorePreviousTarget();
            return;
        }

        lookTarget = hit.collider.GetComponentInParent<Interactable>();

        if (lookTarget == null)
        {
            RestorePreviousTarget();
            return;
        }

        if (!lookTarget.CanInteract(this))
        {
            RestorePreviousTarget();
            lookTarget.Hide();
            return;
        }

        // Yang di-look tapi sudah keluar radius detect
        if (!nearbyObjects.Contains(lookTarget))
        {
            RestorePreviousTarget();
            return;
        }

        // Target berubah
        if (previousLookTarget != null &&
            previousLookTarget != lookTarget &&
            nearbyObjects.Contains(previousLookTarget))
        {
            if (previousLookTarget.CanInteract(this))
                previousLookTarget.ShowDot();
            else
                previousLookTarget.Hide();
        }

        // Dalam jarak interaksi?
        if (hit.distance <= lookTarget.InteractionDistance)
        {
            lookTarget.ShowKey();
            interactTarget = lookTarget;
        }
        else
        {
            lookTarget.ShowDot();
        }

        previousLookTarget = lookTarget;
    }

    private void RestorePreviousTarget()
    {
        if (previousLookTarget != null &&
            nearbyObjects.Contains(previousLookTarget))
        {
            if (previousLookTarget.CanInteract(this))
                previousLookTarget.ShowDot();
            else
                previousLookTarget.Hide();
        }

        previousLookTarget = null;
    }

    #endregion

    #region Input

    private void HandleInput()
    {
        if (!GameStateManager.Instance.IsState(GameState.Exploration))
        {
            if (inputHandler.DialogueNextPressed)
            {
                DialogueManager.Instance.ContinueDialogue();
            }

            inputHandler.ConsumeInput();
            return;
        }

        if (inputHandler.InteractPressed)
        {
            interactTarget?.Interact(this);
        }

        if (inputHandler.DropPressed)
        {
            PlayerCarry.Drop();
        }

        inputHandler.ConsumeInput();
    }

    public void Unregister(Interactable interactable)
    {
        nearbyObjects.Remove(interactable);

        if (previousLookTarget == interactable)
            previousLookTarget = null;

        if (interactTarget == interactable)
            interactTarget = null;

        interactable.Hide();
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectDistance);
    }
}