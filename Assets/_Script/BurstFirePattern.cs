using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstFirePattern", menuName = "Shooting Patterns/Burst Fire")]
public class BurstFirePattern : ShootingPattern
{
    [SerializeField] private int bulletCount = 3;
    [SerializeField] private float delayBetweenShots = 0.2f;

    public override void Fire(MonoBehaviour enemyController, Transform firePoint, GameObject bulletPrefab)
    {
        enemyController.StartCoroutine(BurstCoroutine(firePoint, bulletPrefab));
    }

    private IEnumerator BurstCoroutine(Transform firePoint, GameObject bulletPrefab)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;

            for (int i = 0; i < bulletCount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<EnemyBullet>().SetDirection(directionToPlayer);
                yield return new WaitForSeconds(delayBetweenShots);
            }
        }
    }
}