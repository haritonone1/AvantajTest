using System;
using UnityEngine;

public sealed class OnFootController : MonoBehaviour, IInputReceiver
{
    [SerializeField] private OnFootMovement movement;
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private PlayerInteractor interactor;
    private InteractCanvas interactCanvas;

    private void Start()
    {
        interactCanvas = FindAnyObjectByType<InteractCanvas>();
    }

    public void ReceiveInput(IInputSource input)
    {
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
    }
}