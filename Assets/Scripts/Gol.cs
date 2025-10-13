using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gol : MonoBehaviour
{
    public bool golDoJogador1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(golDoJogador1 == true)
        {
            FindObjectOfType<GameManager>().AumentarPontuacaoDoJogador2();
            other.gameObject.transform.position = Vector2.zero;
        }
        else
        {
            FindObjectOfType<GameManager>().AumentarPontuacaoDoJogador1();
            other.gameObject.transform.position = Vector2.zero;
        }
    }
}