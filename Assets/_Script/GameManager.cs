using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemiesOnScreen = 2;
    [SerializeField] private float respawnDelay;

    [Header("Spawn Area")]
    [SerializeField] private float minSpawnY = 0f;
    [SerializeField] private float maxSpawnY = 0.9f;
    [SerializeField] private float spawnXPosition = 1.25f;

    private int currentEnemyCount = 0;

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
        for (int i = 0; i < maxEnemiesOnScreen; i++)
        {
            SpawnEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerLosesLife()
    {

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
}
