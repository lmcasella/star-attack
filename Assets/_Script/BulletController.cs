using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Prevenir que las balas salgan mas juntas o mas separadas dependiendo de la direccicon de la nave
    public void InitializeBullet(Vector2 shipVelocity)
    {
        // Velocidad de la bala hacia adelante
        Vector2 bulletVelocity = transform.up * speed;

        // + velocidad actual de la nave
        rb.velocity = bulletVelocity + shipVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }

            // Eliminar bala
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boundaries"))
        {
            Destroy(gameObject);
        }
    }
}
