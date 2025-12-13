using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class NetworkBootstrap : MonoBehaviour
{
    [SerializeField] private string gameScene = "RV_TEST";

    public void StartHostAndLoad()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
