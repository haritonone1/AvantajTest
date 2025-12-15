using Unity.Netcode;
using UnityEngine;

public sealed class VanNetworkController : NetworkBehaviour
{
    [SerializeField] private CarMovingModule car;

    private ulong driverClientId = ulong.MaxValue;
    private bool engineOn;
    
    public override void OnNetworkSpawn()
    {
        // Rigidbody активен ТОЛЬКО на сервере
        var rb = GetComponent<Rigidbody>();
        if (rb)
            rb.isKinematic = !IsServer;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendInputServerRpc(
        float throttle,
        float steer,
        bool ignition,
        bool exit,
        ServerRpcParams rpcParams = default)
    {
        if (!IsServer) return;

        if (rpcParams.Receive.SenderClientId != driverClientId)
            return;

        if (exit)
        {
            HandleExit(rpcParams.Receive.SenderClientId);
            return;
        }

        if (!engineOn)
        {
            if (ignition)
                engineOn = true;
            return;
        }

        bool brake = Mathf.Abs(throttle) < 0.01f;
        car.SetInput(throttle, steer, brake);
    }
    
    private void HandleExit(ulong clientId)
    {
        engineOn = false;
        car.StopInstant();

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
            return;

        var playerObject = client.PlayerObject;
        if (playerObject == null) return;

        var workstationManager = playerObject.GetComponent<WorkstationManager>();
        if (workstationManager == null) return;

        workstationManager.ExitServerRpc();

        driverClientId = ulong.MaxValue;
    }

    public void SetDriver(ulong clientId)
    {
        if (!IsServer) return;
        driverClientId = clientId;
    }

    public void ClearDriver()
    {
        if (!IsServer) return;
        driverClientId = ulong.MaxValue;
        engineOn = false;
        car.StopInstant();
    }

    private void FixedUpdate()
    {
        if (!IsServer) return;
        // CarMovingModule сам в FixedUpdate
        // тут ничего не нужно
    }
}