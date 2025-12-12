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
        var rb = GetComponent<Rigidbody>();

        if (!IsOwner)
        {
            rb.isKinematic = true;
            inputSource.enabled = false;
            cameraController.Disable();
            return;
        }

        rb.isKinematic = false;
        cameraController.Enable();
        inputRouter.Initialize(inputSource);
        inputRouter.SetReceiver(onFootController);
    }


}