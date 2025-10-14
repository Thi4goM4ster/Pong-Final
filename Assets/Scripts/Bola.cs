using UnityEngine;

public class Bola : MonoBehaviour
{
    public Vector3 posicaoRede;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, posicaoRede, Time.deltaTime * 10f);
    }
}
