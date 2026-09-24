using UnityEngine;
using System.Collections;
using Game.Flow;

public class TriggerCsAP1 : MonoBehaviour
{
    [SerializeField] private Collider col;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ada object masuk: " + other.name);

        if (other.CompareTag("Player"))
        {
            TransitionManager.Instance.PlayTransition();
            StartCoroutine(BridgeWait());
        }
    }

    private void Start()
    {
        col.enabled = false;
    }

    private IEnumerator BridgeWait()
    {
        yield return new WaitForSeconds(3f);

        FlowEventManager.Instance.Raise("TriggeredToCsAP1");
        col.enabled = false;
    }

    public void Activate()
    {
        col.enabled = true;
    }
}
