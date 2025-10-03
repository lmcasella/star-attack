using UnityEngine;

public abstract class ShootingPattern : ScriptableObject
{
    // Todos los patrones deben tener una funcion Fire, pero cada uno lo maneja distinto
    // Le pasamos el EnemyController asi el patron lo puede usar para empezar las corrutinas
    public abstract void Fire(MonoBehaviour enemyController, Transform firePoint, GameObject bulletPrefab);
}