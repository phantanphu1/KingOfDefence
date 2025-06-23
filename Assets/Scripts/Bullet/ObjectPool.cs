
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    None = 0,
    Normal = 1,
}
public enum EnemyType
{
    None = 0,
    Normal = 1,
}
public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefabs;
    [SerializeField] GameObject enemyPrefabs;
    private Dictionary<BulletType, List<GameObject>> bulletPools = new Dictionary<BulletType, List<GameObject>>();
    private Dictionary<EnemyType, List<GameObject>> enemyPools = new Dictionary<EnemyType, List<GameObject>>();

    public GameObject GetBulletByType(BulletType bulletType)
    {
        if (bulletType == BulletType.None)
        {
            Debug.LogError("");
            return null;
        }
        if (bulletPools.ContainsKey(bulletType))
        {
            var lsBullet = bulletPools[bulletType];
            foreach (var item in lsBullet)
            {
                if (!item.activeInHierarchy)
                {
                    return item;
                }
            }
            return InstantiateBulletByType(bulletType);
        }
        else
        {
            return InstantiateBulletByType(bulletType);
        }


    }
    private GameObject InstantiateBulletByType(BulletType bulletType)
    {
        var obj = InstantiateNormalBullet();
        if (bulletPools.ContainsKey(bulletType))
        {
            var lsBullet = bulletPools[bulletType];
            lsBullet.Add(obj);
        }
        else
        {
            List<GameObject> lsBullet = new List<GameObject>();
            lsBullet.Add(obj);
            bulletPools.Add(bulletType, lsBullet);
        }
        return obj;
    }

    private GameObject InstantiateNormalBullet()
    {
        return Instantiate(bulletPrefabs);
    }


    public GameObject GetEnemyByType(EnemyType enemyType)
    {
        if (enemyPools.ContainsKey(enemyType))
        {

            var lsEnemy = enemyPools[enemyType];
            foreach (var item in lsEnemy)
            {
                if (!item.activeInHierarchy)
                {
                    return item;
                }
            }
            return InstantiateEnemyByType(enemyType);
        }
        else
        {
            return InstantiateEnemyByType(enemyType);
        }
    }
    private GameObject InstantiateEnemyByType(EnemyType enemyType)
    {
        var obj = InstantiateNormalEnemy();
        if (enemyPools.ContainsKey(enemyType))
        {
            var lsEnemy = enemyPools[enemyType];
            lsEnemy.Add(obj);
        }
        else
        {
            List<GameObject> lsEnemy = new List<GameObject>();
            lsEnemy.Add(obj);
            enemyPools.Add(enemyType, lsEnemy);
        }
        return obj;
    }

    private GameObject InstantiateNormalEnemy()
    {
        return Instantiate(enemyPrefabs);
    }
}
