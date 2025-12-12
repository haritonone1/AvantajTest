using System;
using UnityEngine;

using UnityEngine;

public sealed class PlayerInputSource : MonoBehaviour, IInputSource
{
    private InputSystem_Actions actions;

    public Vector2 Move => actions.Player.Move.ReadValue<Vector2>();
    public Vector2 Look => actions.Player.Look.ReadValue<Vector2>();
    public bool Interact => actions.Player.Interact.WasPressedThisFrame();
    public bool Sprint => actions.Player.Sprint.IsPressed();
    public bool Jump => actions.Player.Jump.WasPressedThisFrame();

    private void Awake()
    {
        actions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }
}


public interface IInputReceiver
{
    void ReceiveInput(IInputSource input);
}

public interface IInputSource
{
    Vector2 Move { get; }
    Vector2 Look { get; }
    bool Interact { get; }
    bool Sprint { get; }
    bool Jump { get; }
}