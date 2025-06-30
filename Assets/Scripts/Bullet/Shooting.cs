using System;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] ObjectPool objectPool;
    private EnemySpawner enemySpawner;

    private void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();

        InvokeRepeating("ShootBullet", 1f, 10f);
    }

    public void ShootBullet()
    {


        Enemy farthestEnemy = enemySpawner.GetFarthestEnemy();
        if (farthestEnemy != null)
        {
            if (farthestEnemy.IsAtShootingWaypoint)
            {
                Vector2 enemyPosition = farthestEnemy.transform.position;
                GameObject bullet = objectPool.GetBulletByType(BulletType.Normal);
                bullet.SetActive(true);
                bullet.transform.position = firePoint.position;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                Vector2 bulletPosition = firePoint.position;
                // `.normalized` sẽ biến vector này thành một vector đơn vị (độ dài = 1)
                // để nó chỉ biểu thị hướng, không bị ảnh hưởng bởi khoảng cách ban đầu.
                Vector2 moveToEnemy = (enemyPosition - bulletPosition).normalized;
                rb.velocity = moveToEnemy * bulletSpeed;
            }
        }
    }
}
