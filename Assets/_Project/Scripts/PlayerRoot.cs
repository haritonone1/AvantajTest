using Unity.Netcode;
using UnityEngine;

public sealed class PlayerRoot : NetworkBehaviour
{
    [SerializeField] private PlayerInputSource inputSource;
    [SerializeField] private PlayerInputRouter inputRouter;
    [SerializeField] private OnFootController onFootController;
    [SerializeField] private PlayerCameraController cameraController;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            inputSource.enabled = false;
            cameraController.Disable();
            return;
        }

        cameraController.Enable();
        inputRouter.Initialize(inputSource);
        inputRouter.SetReceiver(onFootController);
    }

}