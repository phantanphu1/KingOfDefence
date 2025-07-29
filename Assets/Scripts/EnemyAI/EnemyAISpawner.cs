using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAISpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public EnemyAI enemyPrefab;
    public Transform spawnPoint;

    [Tooltip("Kéo GameObject chứa script WaypointPath (hoặc parent của các waypoint) vào đây.")]
    public Transform globalPatrolPathParent;

    public float initialSpawnDelay = 0.5f;
    public float spawnRepeatRate = 1f;
    public List<EnemyAI> lsEnemyAI = new List<EnemyAI>();
    // Trong EnemySpawner.cs
    [SerializeField] ObjectPool objectPool;
    [SerializeField] EnemyItemScriptableObject enemyItemConfig;

    void Start()
    {
        InvokeRepeating("SpawnNewEnemy", initialSpawnDelay, spawnRepeatRate);
    }
    void Update()
    {
        GetFarthestEnemy();
    }

    void SpawnNewEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyItemConfig.lsEnemyItem.Count);
        EnemyItem selectedEnemyItem = enemyItemConfig.lsEnemyItem[randomEnemyIndex];
        if (enemyPrefab != null && spawnPoint != null && globalPatrolPathParent != null && objectPool != null)
        {
            // Lấy GameObject enemy từ Object Pool
            GameObject enemyGO = objectPool.GetEnemyAIByType(EnemyType.Normal);

            // Đặt vị trí và Quaternion (xoay) cho enemy vừa lấy ra
            enemyGO.transform.position = spawnPoint.position;
            enemyGO.transform.rotation = Quaternion.identity;
            enemyGO.SetActive(true); // Kích hoạt GameObject nếu nó đang bị tắt

            // Lấy script Enemy từ GameObject
            EnemyAI enemyScript = enemyGO.GetComponent<EnemyAI>();
            if (enemyScript != null)
            {
                // Thêm script Enemy vào danh sách quản lý của Spawner
                lsEnemyAI.Add(enemyScript); // --- Thay đổi ở đây: thêm enemyScript vào list ---

                // Gán patrolPathParent cho script Enemy của enemy mới
                enemyScript.patrolPathParent = globalPatrolPathParent;
                enemyScript.SetUpMovement(selectedEnemyItem.movementSpeed);
                // Reset quãng đường di chuyển của enemy khi nó được sinh ra/tái sử dụng
                enemyScript.ResetData();
            }
            else
            {
                Debug.LogWarning("Enemy prefab được trả về từ pool không có script Enemy hoặc script đã bị gỡ bỏ!");
                enemyGO.SetActive(false); // Tắt nó lại nếu không sử dụng được
            }
        }
        else
        {
            Debug.LogError("Thiếu Prefab kẻ địch, điểm spawn, Global Patrol Path Parent, hoặc ObjectPool trong Spawner!");
            CancelInvoke("SpawnNewEnemy");
        }
    }

    public EnemyAI GetFarthestEnemy()
    {
        EnemyAI farthestEnemy = null;
        float maxDistance = -1f;

        foreach (var enemyGO in lsEnemyAI)
        {

            if (enemyGO != null)
            {
                float currentDistance = enemyGO.GetTotalDistanceTraveled();
                if (currentDistance > maxDistance)
                {
                    maxDistance = currentDistance;
                    farthestEnemy = enemyGO;
                }
            }
        }
        return farthestEnemy;
    }

}
