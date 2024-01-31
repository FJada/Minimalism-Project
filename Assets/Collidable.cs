using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public int SpriteColorIndex;
    public Color SpriteColor { get { return GameController.Colors[SpriteColorIndex]; } }
    SpriteRenderer SpriteRenderer;
    Collider2D Collider;

    public bool IsDead { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.color = SpriteColor;
        Collider = GetComponent<Collider2D>();
        gameObject.layer = LayerMask.NameToLayer($"Color{SpriteColorIndex}");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            transform.localScale *= 0.99f;
        }
        if (transform.localScale.magnitude < 0.1 || transform.position.x < -8)
        {
            Destroy(gameObject);
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(GameController.MovementForce * Time.deltaTime * Vector2.left);
        if (rb.velocity.x >= 0)
        {
            rb.velocity = Vector2.zero;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpriteColorIndex = (SpriteColorIndex + 1) % GameController.Colors.Length;
            SpriteRenderer.color = SpriteColor;
            gameObject.layer = LayerMask.NameToLayer($"Color{SpriteColorIndex}");
        }
    }

    public void Die()
    {
        if (IsDead) return;
        IsDead = true;
        GetComponentInChildren<ParticleSystem>().Play();
    }
}
