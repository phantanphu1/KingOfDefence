using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyItemConfig", menuName = "Enemy/ListEnemyItem")]
public class EnemyItemScriptableObject : ScriptableObject
{
    public List<EnemyItem> lsEnemyItem;
}
[Serializable]
public class EnemyItem
{
    public GameObject enemyPrefab;
    public string id;
    public string enemyName;
    public float health;
    public float movementSpeed;
    public EnemyType enemyType;


    // Constructor
    public EnemyItem(EnemyItem item)
    {
        this.enemyPrefab = item.enemyPrefab;
        this.id = item.id;
        this.enemyName = item.enemyName;
        this.health = item.health;
        this.movementSpeed = item.movementSpeed;
        this.enemyType = item.enemyType;
    }
}
