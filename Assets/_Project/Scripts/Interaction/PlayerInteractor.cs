using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public sealed class PlayerInteractor : NetworkBehaviour
{
    public IReadOnlyList<IInteractable> CurrentInteractables => currentInteractables;
    
    [Header("Raycast")]
    [SerializeField] private Transform viewOrigin;
    [SerializeField] private float interactDistance = 2.5f;
    [SerializeField] private LayerMask interactMask;

    private readonly List<IInteractable> currentInteractables = new();
    private NetworkObject currentTarget;
    private NetworkObject lastTarget;

    public void LocalTick()
    {
        if (!IsOwner) return;

        currentInteractables.Clear();
        currentTarget = null;

        if (Physics.Raycast(
                viewOrigin.position,
                viewOrigin.forward,
                out RaycastHit hit,
                interactDistance,
                interactMask,
                QueryTriggerInteraction.Ignore))
        {
            currentTarget = hit.collider.GetComponentInParent<NetworkObject>();
            if (currentTarget == null) return;

            currentTarget.GetComponents(currentInteractables);
        }
    }

    public void TryInteract(int index)
    {
        if (!IsOwner) return;
        if (index < 0 || index >= currentInteractables.Count) return;

        TryInteractServerRpc(currentTarget.NetworkObjectId, index);
    }

    [ServerRpc]
    private void TryInteractServerRpc(ulong targetId, int index)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects
                .TryGetValue(targetId, out var target))
            return;

        var interactables = target.GetComponents<IInteractable>();

        if (index >= interactables.Length) return;

        var interactable = interactables[index];

        if (!interactable.CanInteract(NetworkObject)) return;

        interactable.Interact(NetworkObject);
    }
    
    public IReadOnlyList<InteractionUIData> GetUIData()
    {
        var list = new List<InteractionUIData>();

        for (int i = 0; i < currentInteractables.Count && i < 3; i++)
        {
            list.Add(new InteractionUIData
            {
                Key = i switch
                {
                    0 => "E",
                    1 => "F",
                    2 => "G",
                    _ => ""
                },
                Description = currentInteractables[i].GetDescription()
            });
        }

        return list;
    }
}

public interface IInteractable
{
    string GetDescription();
    bool CanInteract(NetworkObject player);
    void Interact(NetworkObject player);
}

public struct InteractionUIData
{
    public string Key;
    public string Description;
}

[System.Serializable]
public class InteractionDefinition
{
    [TextArea]
    public string Description;
}
