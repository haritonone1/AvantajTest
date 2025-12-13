using UnityEngine;

public sealed class PlayerInputRouter : MonoBehaviour
{
    private IInputReceiver receiver;
    private IInputSource input;

    public void Initialize(IInputSource inputSource)
    {
        input = inputSource;
    }

    public void SetReceiver(IInputReceiver newReceiver)
    {
        receiver = newReceiver;
    }

    private void Update()
    {
        receiver?.ReceiveInput(input);
    }
}