using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int pontuacaoDoJogador1;
    public int pontuacaoDoJogador2;
    public Text textoDePontuacao;
    public AudioSource somDoGol;

    void Start()
    {
        Cursor.visible = false;
        AtualizarTexto();
    }

    public void AtualizarPlacar(int novoP1, int novoP2)
    {
        pontuacaoDoJogador1 = novoP1;
        pontuacaoDoJogador2 = novoP2;
        AtualizarTexto();
    }

    void AtualizarTexto()
    {
        textoDePontuacao.text = pontuacaoDoJogador1 + " X " + pontuacaoDoJogador2;
        if (somDoGol != null) somDoGol.Play();
    }
}
