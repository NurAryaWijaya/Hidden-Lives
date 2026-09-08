using Game.Flow;
using UnityEngine;

public class TriggerGameOver : MonoBehaviour, IFlowActivatable
{
    [SerializeField] private RenoStealthTask stealth;
    [SerializeField] private Collider box;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ADA YANG MASUK: " + other.name);
        if (other.CompareTag("Player"))
        {
            stealth.GameOver();
            return;
        }
    }

    private void Start()
    {
        box.enabled = false;
    }

    public void Activate()
    {
        box.enabled = true;
    }

    public void Deactivate()
    {
        box.enabled = false;
    }
}
