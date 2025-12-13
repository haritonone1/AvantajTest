using Unity.Netcode;
using UnityEngine;

public sealed class WorkstationManager : NetworkBehaviour
{
    private IWorkstation current;

    public bool IsInWorkstation => current != null;

    // ===== ENTER (SERVER) =====
    public void EnterServer(IWorkstation workstation)
    {
        if (!IsServer) return;

        current = workstation;

        current.OnServerEnter(NetworkObject);
        EnterClientRpc();
    }

    // ===== EXIT (CLIENT -> SERVER) =====
    [ServerRpc]
    public void ExitServerRpc()
    {
        if (current == null) return;

        current.OnServerExit(NetworkObject);
        ExitClientRpc();

        current = null;
    }

    // ===== CLIENT =====
    [ClientRpc]
    private void EnterClientRpc()
    {
        current?.OnClientEnter(NetworkObject);
    }

    [ClientRpc]
    private void ExitClientRpc()
    {
        current?.OnClientExit(NetworkObject);
    }
}

public interface IWorkstation
{
    void OnServerEnter(NetworkObject player);
    void OnClientEnter(NetworkObject player);

    void OnServerExit(NetworkObject player);
    void OnClientExit(NetworkObject player);
}


public interface IWorkstationInteractable : IInteractable
{
    IWorkstation GetWorkstation();
}

