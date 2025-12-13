using UnityEngine;
using UnityEngine.UI;

public sealed class ConnectionCanvas : MonoBehaviour
{
    [SerializeField] private NetworkBootstrap bootstrap;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(OnHostClicked);
        clientButton.onClick.AddListener(OnClientClicked);
    }

    private void OnHostClicked()
    {
        bootstrap.StartHostAndLoad();
        gameObject.SetActive(false);
    }

    private void OnClientClicked()
    {
        bootstrap.StartClient();
        gameObject.SetActive(false);
    }
}