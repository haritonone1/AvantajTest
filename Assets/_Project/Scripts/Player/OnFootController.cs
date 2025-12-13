using System;
using Unity.Netcode;
using UnityEngine;

public sealed class OnFootController : NetworkBehaviour, IInputReceiver
{
    [SerializeField] private OnFootMovement movement;
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private PlayerInteractor interactor;
    [SerializeField] private WorkstationManager workstationManager;
    private InteractCanvas interactCanvas;

    public void ReceiveInput(IInputSource input)
    {
        if(!interactCanvas) interactCanvas = FindAnyObjectByType<InteractCanvas>();
        if(cameraController != null) cameraController.Rotate(input.Look);
        if(movement != null) movement.Move(input.Move, input.Sprint);

        if (movement != null && input.Jump)
            movement.Jump();

        if (interactor != null)
        {
            interactor.LocalTick();

            interactor.LocalTick();

            if (input.InteractPrimary)
                interactor.TryInteract(0);

            if (input.InteractSecondary)
                interactor.TryInteract(1);

            if (input.InteractTertiary)
                interactor.TryInteract(2);

            if (interactCanvas != null) interactCanvas.UpdateUI(interactor.GetUIData());
        }
        
        if (input.Back && workstationManager.IsInWorkstation)
        {
            workstationManager.ExitServerRpc();
        }
    }
}