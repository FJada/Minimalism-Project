using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float MovementForce = 15f;
    public static float MovementAccelerationFactor = .08f;
    public static float Score;
    public static float SpawnInterval = 0.5f;
    public static Color[] Colors = new[] { Color.white, Color.black };


    public PlayerController Player;
    public Collidable ObstaclePrefab;
    public Transform SpawnLocation;
    public ParticleSystem BackgroundParticleSystem;
    public Camera MainCamera;

    private float spawnTimer = 0;
    private List<Collidable> collidables;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.IsDead) {
            MovementForce *= 0.5f;
            BackgroundParticleSystem.Pause();
            MovementForce = 0;
            return;
        }
        spawnTimer += Time.deltaTime;
        if (spawnTimer > SpawnInterval)
        {
            Collidable obj = Instantiate(ObstaclePrefab, SpawnLocation.position + new Vector3(0, Random.Range(-5f, 5f), 0), Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().velocity = Vector2.left * 5f;
            obj.SpriteColorIndex = Random.Range(0, Colors.Length);
            spawnTimer = 0;
        }

        MovementForce *= 1 + MovementAccelerationFactor * Time.deltaTime;
        Score += -MovementForce * Time.deltaTime;
    }
}
