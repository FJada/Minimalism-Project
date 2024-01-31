using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public int SpriteColorIndex;
    public Color SpriteColor { get { return GameController.Colors[SpriteColorIndex]; } }
    SpriteRenderer SpriteRenderer;
    Collider2D Collider;
    AudioSource AudioSource;

    public bool IsDead { get; private set; } = false;

    private float[] deathClipStartTimes = new []{ 0, 3f, 7f };

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.color = SpriteColor;
        Collider = GetComponent<Collider2D>();
        gameObject.layer = LayerMask.NameToLayer($"Color{SpriteColorIndex}");
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
        {
            transform.localScale *= 0.95f;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(GameController.MovementForce * Time.deltaTime * Vector2.left);
        if (rb.velocity.x >= 0)
        {
            rb.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !GameController.GameOver)
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
        StartCoroutine(PostDeath());
    }

    public IEnumerator PostDeath()
    {
        
        AudioSource.time = deathClipStartTimes[Random.Range(0, deathClipStartTimes.Length)];
        AudioSource.Play();
        AudioSource.pitch = 1.5f;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        AudioSource.Stop();
        yield return null;
    }
}
