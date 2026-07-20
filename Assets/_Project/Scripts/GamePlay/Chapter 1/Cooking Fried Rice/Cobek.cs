using Game.Flow;
using UnityEngine;

public class Cobek : MultiPlaceInteractable
{
    [SerializeField]
    private string complitedId;

    protected override void OnCompleted()
    {
        Debug.Log("Mainkan Cutscene");
        FlowEventManager.Instance.Raise(complitedId);
        
    }
}
