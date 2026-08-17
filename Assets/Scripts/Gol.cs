using UnityEngine;
using Unity.Netcode;

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
        if (!NetworkManager.Singleton.IsServer)
            return;

        if (!outro.CompareTag("Bola"))
            return;

        int p1 = gameManager.pontuacaoDoJogador1;
        int p2 = gameManager.pontuacaoDoJogador2;

        if (pontoParaJogador1)
        {
            p1++;
        }
        else
        {
            p2++;
        }

        gameManager.AtualizarPlacar(p1, p2);

        if (bola != null)
        {
            bola.ReiniciarBola();
        }
    }
}