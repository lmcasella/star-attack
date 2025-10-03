using UnityEngine;

[CreateAssetMenu(fileName = "SingleShotPattern", menuName = "Shooting Patterns/Single Shot")]
public class SingleShotPattern : ShootingPattern
{
    public override void Fire(MonoBehaviour enemyController, Transform firePoint, GameObject bulletPrefab)
    {
        // Encontrar al jugador (optimizar despues)
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return;

        Vector2 directionToPlayer = (player.transform.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().SetDirection(directionToPlayer);
    }
}