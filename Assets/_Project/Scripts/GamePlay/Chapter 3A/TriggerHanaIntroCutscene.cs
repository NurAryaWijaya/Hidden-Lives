using Game.Flow;
using UnityEngine;

public class TriggerHanaIntroCutscene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FlowEventManager.Instance.Raise("HanaIntroTriggered");
        }
    }
}
