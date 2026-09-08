using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public void TriggerTransition()
    {
        if (TransitionManager.Instance == null)
        {
            Debug.LogError("TransitionManager tidak ditemukan.");
            return;
        }

        TransitionManager.Instance.PlayTransition();
    }
}