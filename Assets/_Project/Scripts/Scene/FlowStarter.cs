using Game.Flow;
using Unity.VectorGraphics;
using UnityEngine;

public class FlowStarter : MonoBehaviour
{
    [Header("Flow")]
    [SerializeField]
    private FlowGraph startGraph;

    public void StartGame()
    {
        if (startGraph == null)
        {
            Debug.LogError("FlowStarter : Start Graph belum diisi.");
            return;
        }

        if (FlowManager.Instance.CurrentGraph != null)
        {
            Debug.LogWarning("Flow sudah berjalan.");
            return;
        }

        FlowManager.Instance.StartFlow(startGraph);
    }
}
