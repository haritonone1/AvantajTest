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
    }

    public override void OnServerExit(NetworkObject player)
    {
        if (occupiedBy == player.OwnerClientId)
            occupiedBy = ulong.MaxValue;
    }

    // ---------- CLIENT ----------
    public override void OnClientEnter(NetworkObject player)
    {
        if (!player.IsOwner) return;

        var rb = player.GetComponent<Rigidbody>();
        var router = player.GetComponent<PlayerInputRouter>();
        var onFoot = player.GetComponent<OnFootController>();

        rb.isKinematic = true;
        player.transform.SetPositionAndRotation(
            seatPoint.position,
            seatPoint.rotation
        );

        onFoot.enabled = false;
        router.SetReceiver(vanController);
    }

    public override void OnClientExit(NetworkObject player)
    {
        if (!player.IsOwner) return;

        var rb = player.GetComponent<Rigidbody>();
        var router = player.GetComponent<PlayerInputRouter>();
        var onFoot = player.GetComponent<OnFootController>();

        rb.isKinematic = false;
        onFoot.enabled = true;
        router.SetReceiver(onFoot);
    }
}