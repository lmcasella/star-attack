using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float health;
    [SerializeField] private int scoreValue = 100;

    [Header("Shooting")]
    [SerializeField] private ShootingPattern shootingPattern;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Vector2 fireRateRange = new Vector2();
    private float fireTimer;

    [Header("Audio")]
    [SerializeField] private AudioClip killSound;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Debug.Log("Enemy speed is: " + speed);

        if (transform.position.x > 0)
        {
            rb.velocity = Vector2.left * speed;
        }
        else
        {
            rb.velocity = Vector2.right * speed;
        }

        ResetFireTimer();

        Debug.Log("Setting velocity to: " + rb.velocity);
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            if (shootingPattern != null)
            {
                shootingPattern.Fire(this, firePoint, enemyBulletPrefab);
            }
            ResetFireTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundaries"))
        {
            // Al colisionar con una pared de limite, restar 1 de vida al jugador
            GameManager.Instance.PlayerLosesLife();
            // Y destruir el enemigo
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            GameManager.Instance.EnemyDefeated();
            AudioSource.PlayClipAtPoint(killSound, transform.position);
            Destroy(gameObject);
        }
    }

    void ResetFireTimer()
    {
        fireTimer = Random.Range(fireRateRange.x, fireRateRange.y);
        Debug.Log("Next shot in: " + fireTimer + " seconds."); // Add this line

    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyDestroyed();
        }
    }
}
