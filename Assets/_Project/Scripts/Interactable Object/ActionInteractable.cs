using UnityEngine;

public class ActionInteractable : Interactable
{
    public override void Interact(PlayerInteractor player)
    {
        Debug.Log("Object Interacted!");
    }
}