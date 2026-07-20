using Game.Flow;
using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class IngredientVisual
{
    public InteractableType type;
    public GameObject mesh;
}


public class Wok : Interactable, IFlowActivatable
{
    [Header("Ingredient Meshes")]
    [SerializeField] private GameObject eggMesh;
    [SerializeField] private GameObject riceMesh;
    [SerializeField] private GameObject oilMesh;
    [SerializeField] private GameObject seasoningMesh;

    [SerializeField] private GameObject friedRiceMesh;
    [Header("Cooking")]
    [SerializeField] private float cookingTime = 5f;

    private Coroutine cookingCoroutine;
    private bool isCooking;

    private bool isOnPickUp = false;

    protected override void Awake()
    {
        base.Awake();

        eggMesh?.SetActive(false);
        riceMesh?.SetActive(false);
        oilMesh?.SetActive(false);
        seasoningMesh?.SetActive(false);
        friedRiceMesh?.SetActive(false);
    }

    public override void Interact(PlayerInteractor player)
    {
        // Pertama kali menaruh wok
        if (!isOnPickUp)
        {
            FlowEventManager.Instance.Raise("Wok_Interacted");

            isOnPickUp = true;
            return;
        }

        var item = player.PlayerCarry.CurrentItem;

        if (item == null)
            return;

        Debug.Log(item.Type);

        switch (item.Type)
        {
            case InteractableType.Egg:
                Debug.Log("Show Egg");
                eggMesh.SetActive(true);
                FlowEventManager.Instance.Raise("Egg_Added");
                break;

            case InteractableType.Rice:
                Debug.Log("Show Rice");
                riceMesh.SetActive(true);
                FlowEventManager.Instance.Raise("Rice_Added");
                break;

            case InteractableType.Oil:
                Debug.Log("Show Oil");
                oilMesh.SetActive(true);
                FlowEventManager.Instance.Raise("Oil_Added");
                break;

            case InteractableType.Seasoning:
                Debug.Log("Show Seasoning");
                seasoningMesh.SetActive(true);
                FlowEventManager.Instance.Raise("Seasoning_Added");
                break;

            case InteractableType.SoySouce:
                Debug.Log("All Complete");
                FlowEventManager.Instance.Raise("All_Added");
                if (!isCooking)
                {
                    cookingCoroutine = StartCoroutine(CookRice());
                }
                break;

            case InteractableType.Plate:
                Debug.Log("Take With Plate");
                FlowEventManager.Instance.Raise("Take_With_Plate");
                break;
        }

        player.PlayerCarry.Drop();
        Destroy(item.gameObject);
    }

    // Memasak
    private IEnumerator CookRice()
    {
        isCooking = true;

        yield return new WaitForSeconds(cookingTime);

        // Sembunyikan bahan mentah
        eggMesh.SetActive(false);
        riceMesh.SetActive(false);
        oilMesh.SetActive(false);
        seasoningMesh.SetActive(false);

        // Tampilkan nasi goreng
        friedRiceMesh.SetActive(true);

        isCooking = false;

        FlowEventManager.Instance.Raise("FriedRice_Ready");
    }

    // Aktivasi
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void Activate()
    {
        int interactableLayer = LayerMask.NameToLayer("Interactable");

        if (interactableLayer == -1)
        {
            return;
        }

        SetLayerRecursively(gameObject, interactableLayer);
    }

    public void Deactivate()
    {
        int heldObjectLayer = LayerMask.NameToLayer("HeldObject");

        if (heldObjectLayer == -1)
        {
            return;
        }

        SetLayerRecursively(gameObject, heldObjectLayer);
    }
}
