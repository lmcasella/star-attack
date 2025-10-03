using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private List<Transform> firePoints;
    [SerializeField] private float moveSpeed;

    [Header("Audio")]
    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;

        animator.SetFloat("TurnDirection", moveX);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        // Direccion y velocidad
        rb.velocity = moveInput * moveSpeed;

        // Limite de pantalla ejeY
        //Vector3 currentPosition = transform.position;
        //currentPosition.y = Mathf.Clamp(currentPosition.y, -14f, 0f);
        //transform.position = currentPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with an enemy!");

            GameManager.Instance.PlayerLosesLife();

            Destroy(other.gameObject);
        }
    }

    void Shoot()
    {
        foreach (Transform point in firePoints)
        {
            Instantiate(bulletPrefab, point.position, point.rotation);
        }

        audioSource.PlayOneShot(shootSound);
    }
}
