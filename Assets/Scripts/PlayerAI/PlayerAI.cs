using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] ObjectPool objectPool;
    private EnemyAISpawner enemyAISpawner;
    public PlayerAIItem playerAIItem;
    public CellAI currentCell;
    public HeroType HeroType
    {
        get
        {
            if (playerAIItem == null)
            {
                Debug.LogError("playerAIItem is null in PlayerAI! HeroType cannot be retrieved.");
                return default(HeroType);
            }
            return playerAIItem.heroType;
        }
    }
    public int BaseLevel
    {
        get
        {
            if (playerAIItem == null)
            {
                Debug.LogError("playerAIItem is null in PlayerAI! HeroType cannot be retrieved.");
                // return default(); 
            }
            return playerAIItem.baseLevel;
        }
    }
    private void Start()
    {
        enemyAISpawner = FindObjectOfType<EnemyAISpawner>();

        InvokeRepeating("ShootBulletEnemyAI", 1f, 10f);
    }

    public void ShootBulletEnemyAI()
    {


        EnemyAI farthestEnemy = enemyAISpawner.GetFarthestEnemy();
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
    public void SetupPlayerAI(PlayerAIItem playerAIItem)
    {
        this.playerAIItem = playerAIItem;
    }
}
