using Unity.Netcode;
using UnityEngine;

public sealed class VanWorkstation : Workstation
{
    [SerializeField] private Transform seatPoint;
    [SerializeField] private VanController vanController;

    private ulong occupiedBy = ulong.MaxValue;
    public bool IsOccupied => occupiedBy != ulong.MaxValue;

    // ---------- SERVER ----------
    public override void OnServerEnter(NetworkObject player)
    {
        occupiedBy = player.OwnerClientId;

        var vanNetObj = GetComponent<NetworkObject>();
        var playerNetObj = player;

        playerNetObj.TrySetParent(vanNetObj, true);
    }


    public override void OnServerExit(NetworkObject player)
    {
        if (occupiedBy == player.OwnerClientId)
            occupiedBy = ulong.MaxValue;

        player.TryRemoveParent(true);
    }


    public override void OnClientEnter(NetworkObject player)
    {
        if (!player.IsOwner) return;

        var rb = player.GetComponent<Rigidbody>();
        var router = player.GetComponent<PlayerInputRouter>();
        var vanController = GetComponent<VanController>();
        var col = player.GetComponent<Collider>();

        rb.isKinematic = true;
        if (col) col.enabled = false;

        vanController.AttachPlayer(player);
        vanController.PrepareExitTransform(transform, player.transform);
        vanController.OnEnterVan();

        player.transform.localPosition = seatPoint.localPosition;
        player.transform.localRotation = seatPoint.localRotation;

        router.SetReceiver(vanController);
    }


    public override void OnClientExit(NetworkObject player)
    {
        if (!player.IsOwner) return;

        var rb = player.GetComponent<Rigidbody>();
        var router = player.GetComponent<PlayerInputRouter>();
        var onFoot = player.GetComponent<OnFootController>();
        var vehicleController = GetComponent<VanController>();
        var vanController = GetComponent<VanController>();
        var col = player.GetComponent<Collider>();

        vanController.OnExitVan();
        rb.isKinematic = false;
        if (col) col.enabled = true;

        vehicleController.RestoreExitTransform(transform, player.transform);

        router.SetReceiver(onFoot);
    }
}