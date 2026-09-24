using UnityEngine;

public class TriggerInteract : MonoBehaviour
{
    [SerializeField] private Collider col;
    [SerializeField] private TriggerCsAP1 trigger;

    private void Start()
    {
        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ada object masuk: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player masuk!");

            if (trigger != null)
                trigger.Activate();
        }
    }
}