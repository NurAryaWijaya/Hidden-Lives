using System.Linq;
using UnityEngine;

public class MultiPlaceInteractable : MonoBehaviour
{
    [SerializeField] private PlaceInteractable[] places;

    private bool completed;

    private void Awake()
    {
        foreach (var place in places)
        {
            if (place != null)
                place.OnObjectPlaced += CheckCompleted;
        }
    }

    private void OnDestroy()
    {
        foreach (var place in places)
        {
            if (place != null)
                place.OnObjectPlaced -= CheckCompleted;
        }
    }

    protected virtual void CheckCompleted(PlaceInteractable _)
    {
        if (completed)
            return;

        if (!places.All(p => p != null && p.HasObject))
            return;

        completed = true;

        Debug.Log("Semua bahan sudah ditempatkan!");

        OnCompleted();
    }

    protected virtual void OnCompleted()
    {

    }
}