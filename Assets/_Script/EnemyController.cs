using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float health;
    [SerializeField] private int scoreValue = 100;

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

        Debug.Log("Setting velocity to: " + rb.velocity);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            GameManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyDestroyed();
        }
    }
}
