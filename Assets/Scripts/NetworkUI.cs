using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class NetworkUI : MonoBehaviour
{
    [Header("Configuração da rede")]
    [SerializeField] private string ipDoHost = "127.0.0.1";
    [SerializeField] private ushort porta = 7777;

    private UnityTransport transport;

    private void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        }
    }

    public void IniciarHost()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager não encontrado!");
            return;
        }

        if (NetworkManager.Singleton.IsListening)
        {
            Debug.LogWarning("A rede já está iniciada!");
            return;
        }

        transport.SetConnectionData(
            "0.0.0.0",
            porta
        );

        bool iniciou = NetworkManager.Singleton.StartHost();

        Debug.Log("StartHost retornou: " + iniciou);
    }

    public void IniciarClient()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager não encontrado!");
            return;
        }

        if (NetworkManager.Singleton.IsListening)
        {
            Debug.LogWarning("A rede já está iniciada!");
            return;
        }

        transport.SetConnectionData(
            ipDoHost,
            porta
        );

        bool iniciou = NetworkManager.Singleton.StartClient();

        Debug.Log("StartClient retornou: " + iniciou);
    }
}