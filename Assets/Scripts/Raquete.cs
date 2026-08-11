using UnityEngine;
using UnityEngine.InputSystem;

public class Raquete : MonoBehaviour
{
    [SerializeField] private float velocidade = 8f;
    [SerializeField] private bool jogador1 = true;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // A raquete não pode cair nem girar
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        float movimento = 0f;

        if (jogador1)
        {
            // W = sobe
            // S = desce
            if (Keyboard.current.wKey.isPressed)
                movimento = 1f;
            else if (Keyboard.current.sKey.isPressed)
                movimento = -1f;
        }
        else
        {
            // ↑ = sobe
            // ↓ = desce
            if (Keyboard.current.upArrowKey.isPressed)
                movimento = 1f;
            else if (Keyboard.current.downArrowKey.isPressed)
                movimento = -1f;
        }

        rb.linearVelocity = new Vector2(0f, movimento * velocidade);
    }
}