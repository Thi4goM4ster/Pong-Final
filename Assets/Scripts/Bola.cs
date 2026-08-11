using UnityEngine;

public class Bola : MonoBehaviour
{
    [SerializeField] private float velocidade = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Começa indo para uma direção aleatória
        float direcaoX = Random.value < 0.5f ? -1f : 1f;
        float direcaoY = Random.Range(-0.5f, 0.5f);

        Vector2 direcao = new Vector2(direcaoX, direcaoY).normalized;

        rb.linearVelocity = direcao * velocidade;
    }
}
