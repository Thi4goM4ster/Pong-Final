using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Raquete : NetworkBehaviour
{
    [SerializeField] private float velocidade = 8f;
    [SerializeField] private bool jogador1 = true;

    private Rigidbody2D rb;
    private float movimento = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(
            gameObject.name +
            " | Spawnou | Server: " + IsServer +
            " | Client: " + IsClient
        );

        // Só o Host controla a física.
        if (!IsServer)
        {
            rb.simulated = false;
        }
    }

    private void Update()
    {
        if (!IsSpawned)
            return;

        // ==========================
        // JOGADOR 1 - HOST
        // ==========================

        if (jogador1 && IsHost)
        {
            movimento = LerJogador1();

            Debug.Log("HOST - movimento P1: " + movimento);
        }

        // ==========================
        // JOGADOR 2 - CLIENT
        // ==========================

        if (!jogador1 && IsClient && !IsHost)
        {
            float movimentoLocal = LerJogador2();

            Debug.Log("CLIENT - movimento P2: " + movimentoLocal);

            EnviarMovimentoServerRpc(movimentoLocal);
        }
    }

    private void FixedUpdate()
    {
        if (!IsServer)
            return;

        rb.linearVelocity =
            new Vector2(0f, movimento * velocidade);
    }

    private float LerJogador1()
    {
        if (Keyboard.current == null)
            return 0f;

        if (Keyboard.current.wKey.isPressed)
            return 1f;

        if (Keyboard.current.sKey.isPressed)
            return -1f;

        return 0f;
    }

    private float LerJogador2()
    {
        if (Keyboard.current == null)
            return 0f;

        if (Keyboard.current.upArrowKey.isPressed)
            return 1f;

        if (Keyboard.current.downArrowKey.isPressed)
            return -1f;

        return 0f;
    }

    [ServerRpc(RequireOwnership = false)]
    private void EnviarMovimentoServerRpc(float novoMovimento)
    {
        movimento = novoMovimento;

        Debug.Log("HOST recebeu movimento do P2: " + novoMovimento);
    }
}