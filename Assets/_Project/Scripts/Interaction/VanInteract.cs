using Unity.Netcode;
using UnityEngine;

public sealed class VanInteract : NetworkBehaviour, IInteractable
{
    [SerializeField] private VanWorkstation workstation;
    [SerializeField] private string description = "Drive van";

    public string GetDescription() => description;

    public bool CanInteract(NetworkObject player)
    {
        return workstation != null && !workstation.IsOccupied;
    }

    public void Interact(NetworkObject player)
    {
        if (workstation == null) return;

        if (workstation.IsOccupied)
            return;

        var manager = player.GetComponent<WorkstationManager>();
        manager.EnterServer(workstation);
    }
}