using Unity.Netcode;
using UnityEngine;

public sealed class NetworkBootstrap : MonoBehaviour
{
    private void Start()
    {
        if (Application.isEditor)
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}