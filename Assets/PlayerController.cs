using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int SpriteColorIndex = 0;
    public Color SpriteColor { get { return GameController.Colors[SpriteColorIndex]; } }
    public CameraController Camera;

    SpriteRenderer SpriteRenderer;
    ParticleSystem ParticleSystem;
    AudioSource AudioSource;
    Collider2D Collider;

    public bool IsDead { get; private set; } = false; 

    private float cosX = 0;
    private float moveFreq = 2;
    private float moveHeight = 8;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
        Collider = GetComponent<Collider2D>();
        AudioSource = GetComponent<AudioSource>();
        var main = ParticleSystem.main;
        main.startColor = new ParticleSystem.MinMaxGradient(SpriteColor);
        SpriteRenderer.color = SpriteColor;
        gameObject.layer = LayerMask.NameToLayer($"Color{SpriteColorIndex}");
        // Collider.excludeLayers = 1 << gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead && transform.localScale.magnitude > 0)
        {
            // Shrink on death
            // transform.localScale -= new Vector3(0.4f, 0.4f, 0.4f) * Time.deltaTime;
            if (transform.localScale.magnitude < 0)
            {
                transform.localScale = Vector3.zero;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !GameController.GameOver)
        {
            AudioSource.time = 0f;
            AudioSource.volume = 0.5f;
            AudioSource.Play();
        }
        transform.Translate(new Vector2(0, ((float)Math.Cos(cosX)) * moveHeight * Time.deltaTime));
        cosX += ((float)Math.PI) * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collidable component;

        // Only proceed if the other object is an obstacle
        if (!collision.gameObject.TryGetComponent(out component))
        {
            return;
        }

        if (component.IsDead)
        {
            return;
        }

        // No issue if the colors match
        if (component.SpriteColor == SpriteColor)
        {
            Camera.Shake(0.5f, 0.15f);
            component.Die();
            GameController.Score += GameController.MovementForce * 2;
            return;
        }
        IsDead = true;
        var rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = 1;
        GetComponentInChildren<ParticleSystem>().Stop();
    }
}
