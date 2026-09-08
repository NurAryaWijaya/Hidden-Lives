using Game.Flow;
using UnityEngine;

public class SitDownCom : MonoBehaviour, IFlowActivatable
{
    [SerializeField] private SitDownTrigger sitDownTrigger;
    public void Activate()
    {
        sitDownTrigger.StartSitDown();
    }

    public void Deactivate()
    {
        sitDownTrigger.CloseComputer();
    }
}
