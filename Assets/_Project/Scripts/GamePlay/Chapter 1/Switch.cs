using Game.Flow;
using UnityEngine;

public class Switch : Interactable, IFlowActivatable
{
    [Header("Switch Handle")]
    [SerializeField] private Transform handle; // Child yang akan diputar

    [SerializeField] private Light[] lights;

    [Header("Rotation Settings")]
    [SerializeField] private float openAngle = 20f;
    [SerializeField] private float openSpeed = 1.5f;

    public bool IsTurnOn { get; private set; }

    private Quaternion turnOffRotation;
    private Quaternion turnOnRotation;

    // Kondisi
    // Chapter 1
    private bool IsFirstTurnOnKitchenLight = true;


    private void Start()
    {
        // Jika belum diisi di Inspector, gunakan transform sendiri
        if (handle == null)
            handle = transform;

        turnOffRotation = handle.localRotation;
        turnOnRotation = turnOffRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        Quaternion target = IsTurnOn ? turnOnRotation : turnOffRotation;

        handle.localRotation = Quaternion.Lerp(
            handle.localRotation,
            target,
            openSpeed * Time.deltaTime
        );
    }

    public override void Interact(PlayerInteractor player)
    {
        IsTurnOn = !IsTurnOn;

        if (IsTurnOn)
        {
            Activate();
            FlowEventManager.Instance.Raise("Turn On Lamp");
        }
        else
        {
            Deactivate();
        }
    }

    private void SetState(bool state)
    {
        IsTurnOn = state;
        UpdateLights();
    }

    private void UpdateLights()
    {
        foreach (Light light in lights)
            light.enabled = IsTurnOn;
    }

    public void Activate()
    {
        SetState(true);

        // Ini ketika pertama kali menyalakan lampu dapur di chapter 1
        if (IsFirstTurnOnKitchenLight)
        {
            FlowEventManager.Instance.Raise("CH1_First_TurnOn_Kitchen_Light");
            IsFirstTurnOnKitchenLight = false;
        }
    }

    public void Deactivate()
    {
        SetState(false);
    }
}