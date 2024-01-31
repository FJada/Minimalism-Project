using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static float MovementForce = 15f;
    public static float MovementAccelerationFactor = .08f;
    public static float Score;
    public static float SpawnInterval = 0.5f;
    public static float MinSpawnInterval = 0.1f;
    public static float SpawnIntervalChangeRate = -0.01f;
    public static Color[] Colors = new[] { Color.white, Color.black };


    public PlayerController Player;
    public Collidable ObstaclePrefab;
    public Collidable ObstaclePrefab2;
    public Collidable ObstaclePrefab3;
    public Transform SpawnLocation;
    public ParticleSystem BackgroundParticleSystem;

    public bool GameOver = false;

    private float spawnTimer = 0;
    private List<Collidable> collidables;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver) 
        {
            return; 
        }

        if (Player.IsDead) {
            GameOver = true;
            MovementForce *= -4;
            BackgroundParticleSystem.Pause();
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer > SpawnInterval)
        {
            int rand = Random.Range(0, 3);
            Collidable prefabToSpawn =  rand== 0 ? ObstaclePrefab : (rand==1 ? ObstaclePrefab2:ObstaclePrefab3);
            Collidable obj = Instantiate(prefabToSpawn, SpawnLocation.position + new Vector3(0, Random.Range(-5f, 5f), 0), Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().velocity = Vector2.left * 5f;
            obj.SpriteColorIndex = Random.Range(0, Colors.Length);
            spawnTimer = 0;
        }

        SpawnInterval += SpawnIntervalChangeRate * Time.deltaTime;
        MovementForce *= 1 + MovementAccelerationFactor * Time.deltaTime;
        Score += -MovementForce * Time.deltaTime;
    }
}
