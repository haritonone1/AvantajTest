using Unity.Netcode;
using UnityEngine;

public sealed class PlayerInteractor : NetworkBehaviour
{
    [Header("Raycast")]
    [SerializeField] private Transform viewOrigin;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private LayerMask interactMask;

    private NetworkObject lastTarget;

    public void LocalTick()
    {
        if (!IsOwner) return;

        lastTarget = null;

        if (Physics.Raycast(
                viewOrigin.position,
                viewOrigin.forward,
                out RaycastHit hit,
                interactDistance,
                interactMask,
                QueryTriggerInteraction.Ignore))
        {
            lastTarget = hit.collider.GetComponentInParent<NetworkObject>();
        }
    }

    public void TryInteract()
    {
        if (!IsOwner) return;
        if (lastTarget == null) return;

        TryInteractServerRpc(lastTarget.NetworkObjectId);
    }

    [ServerRpc]
    private void TryInteractServerRpc(ulong targetId)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects
                .TryGetValue(targetId, out NetworkObject target))
            return;

        if (!target.TryGetComponent<IInteractable>(out var interactable))
            return;

        if (!interactable.CanInteract(NetworkObject))
            return;

        interactable.Interact(NetworkObject);
    }
}


public interface IInteractable
{
    bool CanInteract(NetworkObject player);
    void Interact(NetworkObject player);
}
