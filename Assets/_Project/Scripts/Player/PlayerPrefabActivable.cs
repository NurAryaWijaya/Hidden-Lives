using Game.Flow;
using UnityEngine;

public class PlayerPrefabActivable : MonoBehaviour, IFlowActivatable
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    public void Activate()
    {
        player1.SetActive(true);
        player2.SetActive(false);
    }

    public void Deactivate()
    {
        player1.SetActive(false);
        player2.SetActive(true);
    }
}
