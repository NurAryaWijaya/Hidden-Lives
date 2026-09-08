using Game.Flow;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SitDownTrigger : Interactable, IFlowActivatable
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera computerCamera;
    [SerializeField] private int activePriority = 10;
    [SerializeField] private int inactivePriority = 0;

    [Header("Canvas")]
    [SerializeField] private GameObject computerCanvas;
    [SerializeField] private ComputerProgressBar computerProgressBar;
    [SerializeField] private OnFocusToCOmputer focusToCOmputer;
    [SerializeField] private ComputerMouseLook computerMouseLook;

    private void Start()
    {
        playerCamera.Priority = activePriority;
        computerCamera.Priority = inactivePriority;
    }

    public override void Interact(PlayerInteractor player)
    {
        FlowEventManager.Instance.Raise("SitDownAlert");
    }

    public void StartSitDown()
    {
        GameStateManager.Instance.SetState(GameState.OnComputer);

        playerCamera.Priority = inactivePriority;
        computerCamera.Priority = activePriority;

        StartCoroutine(computerMouseLook.StartControlDelay());
        StartCoroutine(StartCPU());
    }

    public void CloseComputer()
    {
        playerCamera.Priority = activePriority;
        computerCamera.Priority = inactivePriority;

        computerCanvas.SetActive(false);

        GameStateManager.Instance.SetState(GameState.Exploration); 
    }

    private IEnumerator StartCPU()
    {
        yield return new WaitForSeconds(4f);

        computerCanvas.SetActive(true);

        yield return new WaitForSeconds(4f);
        computerProgressBar.StartProgress();

        yield return new WaitUntil(()=> computerProgressBar.IsReady);
        if (computerProgressBar.IsReady == true)
        {
            Debug.Log("Progress bar selesai, lanjut ke trigger onfocus");
            FlowEventManager.Instance.Raise("OSReady");
            computerProgressBar.IsReady = false;
            focusToCOmputer.StartComputer();
        }
    }

    // Aktivasi
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void Activate()
    {
        int interactableLayer = LayerMask.NameToLayer("Interactable");

        if (interactableLayer == -1)
        {
            return;
        }

        SetLayerRecursively(gameObject, interactableLayer);
    }

    public void Deactivate()
    {
        int heldObjectLayer = LayerMask.NameToLayer("HeldObject");

        if (heldObjectLayer == -1)
        {
            return;
        }

        SetLayerRecursively(gameObject, heldObjectLayer);
    }
}
