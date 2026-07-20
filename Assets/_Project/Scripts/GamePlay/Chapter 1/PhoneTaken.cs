using Game.Flow;
using UnityEngine;

public class PhoneTaken : Interactable
{
    private bool isPhoneTaken = false;

    public bool IsPhoneTaken => isPhoneTaken;
    public override void Interact(PlayerInteractor player)
    {
        isPhoneTaken = true;
        Debug.Log("Phone Interacted!");
        FlowEventManager.Instance.Raise("Phone_Taken");
    }
}
