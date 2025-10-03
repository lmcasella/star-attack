using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int enemiesToAddPerWave = 1;
    [SerializeField] private int baseEnemyScore = 100;
    [SerializeField] private GameObject controlsPanel;

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
    //[SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject[] lifeIcons;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuPanel;
    public static bool isPaused = false;

    [Header("Audio")]
    [SerializeField] private AudioClip playerTakeDamageSound;
    private AudioSource audioSource;

    [Header("Wave Transition")]
    [SerializeField] private TextMeshProUGUI waveTransitionText;
    [SerializeField] private float timeBetweenWaves = 3f;

    private int currentEnemyCount = 0;
    public static int finalScore;

    private int score = 0;
    private bool isTransitioningWave = false;

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
        audioSource = GetComponent<AudioSource>();

        waveTimer = waveDuration;

        for (int i = 0; i < maxEnemiesOnScreen; i++)
        {
            SpawnEnemy();
        }

        isPaused = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioningWave)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        if (waveTimer > 0)
        {
            waveTimer -= Time.deltaTime;
        }
        else if (!isTransitioningWave)
        {
            isTransitioningWave = true;
            StartCoroutine(WaveTransition());
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "PUNTAJE: " + score;
        waveText.text = "OLEADA: " + currentWave;

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i < playerLives)
            {
                lifeIcons[i].SetActive(true);
            }
            else
            {
                lifeIcons[i].SetActive(false);
            }
        }

        float minutes = Mathf.FloorToInt(waveTimer / 60);
        float seconds = Mathf.FloorToInt(waveTimer % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator WaveTransition()
    {
        // Pausar juego
        isPaused = true;
        Time.timeScale = 0f;

        if (waveTransitionText == null)
        {
            Debug.LogError("ERROR: Wave Transition Text is NOT ASSIGNED in the GameManager Inspector!");
            // We must un-pause before stopping, or the game will be stuck forever.
            Time.timeScale = 1f;
            isPaused = false;
            yield break; // Stop the coroutine
        }

        // 1: Mostrar mensaje de oleada completa
        waveTransitionText.text = "OLEADA " + (currentWave) + " COMPLETA";
        waveTransitionText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(timeBetweenWaves);

        // Checkear si ganó antes de mostrar la pantalla de victoria
        if (currentWave >= 4)
        {
            finalScore = score;
            Time.timeScale = 1f;
            SceneManager.LoadScene("VictoryScene");
            yield break;
        }

        currentWave++;

        waveTransitionText.text = "PREPARATE PARA LA OLEADA " + currentWave;

        yield return new WaitForSecondsRealtime(timeBetweenWaves / 2);

        // Despausar
        waveTransitionText.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        StartNextWave();

        isTransitioningWave = false;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Tiempo normal de ejecucion
        isPaused = false;
    }

    void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Para el tiempo
        isPaused = true;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void PlayerLosesLife()
    {
        playerLives--;
        audioSource.PlayOneShot(playerTakeDamageSound);
        Debug.Log("El jugador perdió una vida. Vidas restantes: " + playerLives);

        if (playerLives <= 0)
        {
            finalScore = score;
            SceneManager.LoadScene("GameOverScene");
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

    public void EnemyDefeated()
    {
        int scoreToAdd = baseEnemyScore * currentWave;
        AddScore(scoreToAdd);
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
        waveTimer = waveDuration;
        maxEnemiesOnScreen += enemiesToAddPerWave;
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

    public void ShowControls()
    {
        controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        controlsPanel.SetActive(false);
    }
}
