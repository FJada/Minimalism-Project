using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public int SpriteColorIndex;
    public Color SpriteColor { get { return GameController.Colors[SpriteColorIndex]; } }
    SpriteRenderer SpriteRenderer;
    Collider2D Collider;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer.color = SpriteColor;
        Collider = GetComponent<Collider2D>();
        gameObject.layer = LayerMask.NameToLayer($"Color{SpriteColorIndex}");
        Debug.Log(LayerMask.NameToLayer($"Color{SpriteColorIndex}"));
        Collider.excludeLayers = 1 << gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(GameController.MovementForce * Time.deltaTime * Vector2.left);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpriteColorIndex = (SpriteColorIndex + 1) % GameController.Colors.Length;
            SpriteRenderer.color = SpriteColor;
            gameObject.layer = LayerMask.NameToLayer($"Color{SpriteColorIndex}");
            Collider.excludeLayers = 1 << gameObject.layer;
        }
    }
}
