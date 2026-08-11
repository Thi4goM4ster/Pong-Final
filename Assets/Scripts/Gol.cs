using UnityEngine;

public class Gol : MonoBehaviour
{
    [Header("Referência")]
    [SerializeField] private GameManager gameManager;

    [Header("Quem marca o ponto?")]
    [SerializeField] private bool pontoParaJogador1;

    [Header("Bola")]
    [SerializeField] private Bola bola;

    private void OnTriggerEnter2D(Collider2D outro)
    {
        // Verifica se foi a bola que entrou no gol
        if (!outro.CompareTag("Bola"))
            return;

        // Pega a pontuação atual
        int p1 = gameManager.pontuacaoDoJogador1;
        int p2 = gameManager.pontuacaoDoJogador2;

        // Dá o ponto para o jogador correto
        if (pontoParaJogador1)
        {
            p1++;
        }
        else
        {
            p2++;
        }

        // Atualiza o placar
        gameManager.AtualizarPlacar(p1, p2);

        // Reposiciona a bola
        if (bola != null)
        {
            bola.transform.position = Vector3.zero;
        }
    }
}
