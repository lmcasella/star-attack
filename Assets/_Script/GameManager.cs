using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int enemiesToAddPerWave = 1;

    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemiesOnScreen = 2;
    [SerializeField] private float respawnDelay;

    [Header("Spawn Area")]
    [SerializeField] private float minSpawnY = 0f;
    [SerializeField] private float maxSpawnY = 0.9f;
    [SerializeField] private float spawnXPosition = 1.25f;

    [Header("Player Stats")]
    [SerializeField] private int playerLives = 3;

    [Header("Game Loop")]
    [SerializeField] private float waveDuration = 10f; // 300 segundos = 5 minutos
    private float waveTimer;
    private int currentWave = 1;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI waveText;

    private int currentEnemyCount = 0;
    public int score = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        waveTimer = waveDuration;

        for (int i = 0; i < maxEnemiesOnScreen; i++)
        {
            SpawnEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waveTimer > 0)
        {
            waveTimer -= Time.deltaTime;
        }
        else
        {
            StartNextWave();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "SCORE: " + score;
        livesText.text = "LIVES: " + playerLives;
        waveText.text = "WAVE: " + currentWave;

        float minutes = Mathf.FloorToInt(waveTimer / 60);
        float seconds = Mathf.FloorToInt(waveTimer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void PlayerLosesLife()
    {
        playerLives--;
        Debug.Log("El jugador perdió una vida. Vidas restantes: " + playerLives);

        if (playerLives < 0)
        {
            Debug.Log("PERDISTE");
        }
    }

    private void SpawnEnemy()
    {
        // Calcular en que posicion del eje Y va a spawnear el enemigo
        float randomY = Random.Range(minSpawnY, maxSpawnY);

        // De que lado en el eje X va a spawnear el enemigo
        // 0 = Izquierda / 1 = Derecha
        int randomSide = Random.Range(0, 2);
        float spawnX = (randomSide == 0) ? -spawnXPosition : spawnXPosition;

        Vector2 spawnPosition = new Vector2(spawnX, randomY);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
    }

    private IEnumerator SpawnWithDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy();
    }

    public void EnemyDestroyed()
    {
        currentEnemyCount--;

        if (currentEnemyCount < maxEnemiesOnScreen)
        {
            StartCoroutine(SpawnWithDelay());
        }
    }

    void StartNextWave()
    {
        currentWave++;
        waveTimer = waveDuration;
        maxEnemiesOnScreen++;
        Debug.Log("Starting Wave: " + currentWave);

        for (int i = 0; i < enemiesToAddPerWave; i++)
        {
            SpawnEnemy();
        }
    }

    public void AddScore(int pointsToAdd)
    {
        score += pointsToAdd;
        Debug.Log("Score: " + score);
    }
}
