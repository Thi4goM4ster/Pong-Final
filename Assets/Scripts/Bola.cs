using UnityEngine;
using Unity.Netcode;

public class Bola : NetworkBehaviour
{
    [SerializeField] private float velocidade = 5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            rb.simulated = true;
            IniciarBola();
        }
        else
        {
            rb.simulated = false;
        }
    }

    private void IniciarBola()
    {
        float direcaoX = Random.value < 0.5f ? -1f : 1f;
        float direcaoY = Random.Range(-0.5f, 0.5f);

        Vector2 direcao =
            new Vector2(direcaoX, direcaoY).normalized;

        rb.linearVelocity = direcao * velocidade;
    }

    public void ReiniciarBola()
    {
        if (!IsServer)
            return;

        rb.linearVelocity = Vector2.zero;
        transform.position = Vector3.zero;

        IniciarBola();
    }
}