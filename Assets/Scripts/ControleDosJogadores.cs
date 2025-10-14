using UnityEngine;

public class ControleDosJogadores : MonoBehaviour
{
    public float velocidadeDoJogador = 5f;
    public bool isLocalPlayer = false;
    public Vector3 posicaoRede;
    public float yMinimo = -4f, yMaximo = 4f;

    void Update()
    {
        if (isLocalPlayer)
        {
            float v = Input.GetAxis("Vertical");
            transform.Translate(Vector2.up * v * velocidadeDoJogador * Time.deltaTime);
            transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, yMinimo, yMaximo));
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, posicaoRede, Time.deltaTime * 10f);
        }
    }
}
