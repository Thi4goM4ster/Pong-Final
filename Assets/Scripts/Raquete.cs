using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Raquete : NetworkBehaviour
{
    [SerializeField] private float velocidade = 8f;
    [SerializeField] private bool jogador1 = true;

    private Rigidbody2D rb;

    private float movimentoServidor = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    public override void OnNetworkSpawn()
    {
        // Somente o servidor simula a física das raquetes.
        if (!IsServer)
        {
            rb.simulated = false;
        }
    }

    private void FixedUpdate()
    {
        if (!IsSpawned)
            return;

        // CLIENT/HOST envia seu comando para o servidor.
        if (IsClient)
        {
            bool souJogadorLocal;

            if (jogador1)
            {
                // Host = jogador 1
                souJogadorLocal = NetworkManager.Singleton.IsHost;
            }
            else
            {
                // Client = jogador 2
                souJogadorLocal = !NetworkManager.Singleton.IsHost;
            }

            if (souJogadorLocal)
            {
                float movimento = LerTeclas();

                EnviarMovimentoRpc(movimento);
            }
        }

        // SOMENTE o servidor movimenta fisicamente a raquete.
        if (IsServer)
        {
            rb.linearVelocity =
                new Vector2(0f, movimentoServidor * velocidade);
        }
    }

    private float LerTeclas()
    {
        if (jogador1)
        {
            if (Keyboard.current.wKey.isPressed)
                return 1f;

            if (Keyboard.current.sKey.isPressed)
                return -1f;
        }
        else
        {
            if (Keyboard.current.upArrowKey.isPressed)
                return 1f;

            if (Keyboard.current.downArrowKey.isPressed)
                return -1f;
        }

        return 0f;
    }

    [Rpc(SendTo.Server)]
    private void EnviarMovimentoRpc(float movimento)
    {
        movimentoServidor = movimento;
    }
}