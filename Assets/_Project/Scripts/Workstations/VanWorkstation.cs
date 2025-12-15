using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public sealed class VanWorkstation : Workstation
{
    [SerializeField] private Transform seatPoint;
    [SerializeField] private VanController vanController;
    [SerializeField] private VanNetworkController network;
    
    private ulong occupiedBy = ulong.MaxValue;
    public bool IsOccupied => occupiedBy != ulong.MaxValue;
    private ulong driverClientId = ulong.MaxValue;


    public override void OnServerEnter(NetworkObject player)
    {
        occupiedBy = player.OwnerClientId;
        network.SetDriver(player.OwnerClientId);

        player.TrySetParent(GetComponent<NetworkObject>(), true);
    }

    public override void OnServerExit(NetworkObject player)
    {
        if (occupiedBy == player.OwnerClientId)
            occupiedBy = ulong.MaxValue;

        network.ClearDriver();
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
        vanController.OnEnterVan();

        StartCoroutine(PlaceAfterParent(player));

        router.SetReceiver(vanController);
    }

    private IEnumerator PlaceAfterParent(NetworkObject player)
    {
        yield return null;

        player.transform.localPosition = seatPoint.localPosition;
        player.transform.localRotation = seatPoint.localRotation;
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