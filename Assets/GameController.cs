using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static float MovementForce = 15f;
    public static float HighScore = 0;
    public static float Score = 0;
    public static float SpawnInterval = 0.5f;
    public static bool GameOver = false;

    public static float MinSpawnInterval = 0.1f;
    public static float MovementAccelerationFactor = .08f;
    public static float SpawnIntervalChangeRate = -0.01f;
    public static Color[] Colors = new[] { Color.white, Color.black };


    public PlayerController Player;
    public Collidable ObstaclePrefab;
    public Collidable ObstaclePrefab2;
    public Collidable ObstaclePrefab3;
    public Transform SpawnLocation;
    public ParticleSystem BackgroundParticleSystem;
    public TextMeshProUGUI ScoreText;
    public GameObject DeathScreen;
    public TextMeshProUGUI DeathScoreText;
    public TextMeshProUGUI HighScoreText;

    private float spawnTimer = 0;
    private float gameOverDelay = 0f;
    private List<Collidable> collidables;

    // Start is called before the first frame update
    void Start()
    {
        MovementForce = 15f;
        MovementAccelerationFactor = .08f;
        Score = 0;
        SpawnInterval = 0.5f;
        GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver) 
        {
            gameOverDelay -= Time.deltaTime;
            HighScoreText.fontSize = 25 + (Mathf.Sin(Time.time * 5) * 5);
            // To avoid accidental restart upon death
            if (Input.GetKeyUp(KeyCode.Space) && gameOverDelay <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        if (Player.IsDead) {
            GameOver = true;
            MovementForce *= -4;
            BackgroundParticleSystem.Pause();
            ScoreText.gameObject.SetActive(false);
            DeathScoreText.text = $"Score: {((int)Score)}";
            DeathScreen.SetActive(true);
            gameOverDelay = 0.5f;

            if (Score > HighScore)
            {
                HighScoreText.gameObject.SetActive(true);
                HighScore = Score;
            }
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer > SpawnInterval)
        {
            int rand = Random.Range(0, 3);
            Collidable prefabToSpawn =  rand== 0 ? ObstaclePrefab : (rand==1 ? ObstaclePrefab2:ObstaclePrefab3);
            Collidable obj = Instantiate(prefabToSpawn, SpawnLocation.position + new Vector3(0, Random.Range(-5f, 5f), 0), Quaternion.identity);
            var rb = obj.GetComponent<Rigidbody2D>();
            if (MovementForce < 50)
            {
                rb.velocity = Vector2.left * 5f;
            }
            else
            {
                // Add some difficulty
                rb.velocity = new Vector2(-5f, Random.Range(-1f, 1f));
            }
            obj.SpriteColorIndex = Random.Range(0, Colors.Length);
            spawnTimer = 0;
        }

        SpawnInterval += SpawnIntervalChangeRate * Time.deltaTime;
        MovementForce *= 1 + MovementAccelerationFactor * Time.deltaTime;
        Score += MovementForce * Time.deltaTime;
        var displayHighScore = HighScore > Score ? HighScore : Score;
        ScoreText.text = $"Score: {((int)Score)}\nHigh Score: {(int)displayHighScore}";
    }
}
