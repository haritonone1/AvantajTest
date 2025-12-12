using UnityEngine;

public sealed class OnFootController : MonoBehaviour, IInputReceiver
{
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private float speed = 5f;

    public void ReceiveInput(IInputSource input)
    {
        Vector3 move =
            new Vector3(input.Move.x, 0f, input.Move.y);

        transform.Translate(move * speed * Time.deltaTime, Space.Self);
        cameraController.Rotate(input.Look);
    }
}