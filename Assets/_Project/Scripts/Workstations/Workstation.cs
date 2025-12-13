using Unity.Netcode;
using UnityEngine;

public abstract class Workstation : MonoBehaviour, IWorkstation
{
    public abstract void OnServerEnter(NetworkObject player);
    public abstract void OnClientEnter(NetworkObject player);

    public abstract void OnServerExit(NetworkObject player);
    public abstract void OnClientExit(NetworkObject player);
}