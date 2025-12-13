using Unity.Netcode;
using UnityEngine;

public sealed class TestInteractable : NetworkBehaviour, IInteractable
{
    public bool CanInteract(NetworkObject player)
    {
        return true;
    }

    public void Interact(NetworkObject player)
    {
        Debug.Log($"[SERVER] {player.OwnerClientId} interacted with {name}");
    }
}