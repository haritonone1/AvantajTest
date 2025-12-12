using Unity.Netcode;

public sealed class PlayerIdentity : NetworkBehaviour
{
    public ulong ClientId => OwnerClientId;
    public bool IsLocal => IsOwner;
}