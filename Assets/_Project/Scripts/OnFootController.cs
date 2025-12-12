using UnityEngine;

public sealed class OnFootController : MonoBehaviour, IInputReceiver
{
    [SerializeField] private OnFootMovement movement;
    [SerializeField] private PlayerCameraController cameraController;

    public void ReceiveInput(IInputSource input)
    {
        cameraController.Rotate(input.Look);
        movement.Move(input.Move, input.Sprint);

        if (input.Jump)
            movement.Jump();
    }
}