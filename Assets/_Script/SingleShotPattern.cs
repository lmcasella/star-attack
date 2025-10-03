using UnityEngine;

[CreateAssetMenu(fileName = "SingleShotPattern", menuName = "Shooting Patterns/Single Shot")]
public class SingleShotPattern : ShootingPattern
{
    public override void Fire(MonoBehaviour enemyController, Transform firePoint, GameObject bulletPrefab)
    {
        // A simple way to find the player. Can be optimized later.
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return; // Don't fire if the player doesn't exist

        Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().SetDirection(directionToPlayer);
    }
}