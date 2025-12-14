using Unity.Netcode;
using UnityEngine;

public sealed class VanNetworkController : NetworkBehaviour
{
    [SerializeField] private CarMovingModule car;

    private bool engineOn;

    public override void OnNetworkSpawn()
    {
        // Rigidbody активен ТОЛЬКО на сервере
        var rb = GetComponent<Rigidbody>();
        if (rb)
            rb.isKinematic = !IsServer;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetEngineStateServerRpc(bool value)
    {
        engineOn = value;

        if (!engineOn)
            car.StopInstant();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetInputServerRpc(float throttle, float steer)
    {
        if (!engineOn) return;

        bool brake = Mathf.Abs(throttle) < 0.01f;
        car.SetInput(throttle, steer, brake);
    }

    private void FixedUpdate()
    {
        if (!IsServer) return;
        // CarMovingModule сам в FixedUpdate
        // тут ничего не нужно
    }
}