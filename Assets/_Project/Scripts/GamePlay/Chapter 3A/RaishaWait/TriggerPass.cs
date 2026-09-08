using Game.Flow;
using System.Collections;
using UnityEngine;

public class TriggerPass : Interactable, IFlowActivatable
{
    [SerializeField] private Collider trigger;
    [SerializeField] private RenoStealthTask task;
    
    public override void Interact(PlayerInteractor player)
    {
        StartCoroutine(WaitTransition());
    }

    private IEnumerator WaitTransition()
    {
        TransitionManager.Instance.PlayTransition();
        yield return new WaitForSeconds(3f);

        FlowEventManager.Instance.Raise("SuccessPass");
        task.StopQuest();
    }

    private void Start()
    {
        trigger.enabled = false;
    }
    public void Activate()
    {
        trigger.enabled = true;
    }

    public void Deactivate()
    {
        trigger.enabled = false;
    }
}
