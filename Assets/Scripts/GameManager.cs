using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    public NetworkVariable<int> pontuacaoJogador1 =
        new NetworkVariable<int>(0);

    public NetworkVariable<int> pontuacaoJogador2 =
        new NetworkVariable<int>(0);

    public Text textoDePontuacao;
    public AudioSource somDoGol;

    public int pontuacaoDoJogador1
    {
        get { return pontuacaoJogador1.Value; }
    }

    public int pontuacaoDoJogador2
    {
        get { return pontuacaoJogador2.Value; }
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    public override void OnNetworkSpawn()
    {
        pontuacaoJogador1.OnValueChanged += AoMudarPlacar;
        pontuacaoJogador2.OnValueChanged += AoMudarPlacar;

        AtualizarTexto();
    }

    public override void OnNetworkDespawn()
    {
        pontuacaoJogador1.OnValueChanged -= AoMudarPlacar;
        pontuacaoJogador2.OnValueChanged -= AoMudarPlacar;
    }

    public void AtualizarPlacar(int novoP1, int novoP2)
    {
        if (!IsServer)
            return;

        pontuacaoJogador1.Value = novoP1;
        pontuacaoJogador2.Value = novoP2;
    }

    private void AoMudarPlacar(int anterior, int atual)
    {
        AtualizarTexto();

        if (somDoGol != null)
            somDoGol.Play();
    }

    private void AtualizarTexto()
    {
        if (textoDePontuacao == null)
            return;

        textoDePontuacao.text =
            pontuacaoJogador1.Value +
            " X " +
            pontuacaoJogador2.Value;
    }
}