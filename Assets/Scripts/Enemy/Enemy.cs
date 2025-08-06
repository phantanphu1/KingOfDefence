using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{


    [Header("Patrol Settings")]
    [Tooltip("Kéo GameObject cha chứa các điểm waypoint vào đây. Các waypoint con của nó sẽ được sử dụng.")]
    public Transform patrolPathParent; // Tham chiếu đến GameObject cha chứa các waypoint

    [Tooltip("Tốc độ di chuyển của enemy.")]
    public float moveSpeed = 5f;

    [Tooltip("Khoảng cách tối thiểu để coi là đã đến điểm waypoint.")]
    public float arrivalThreshold = 0.1f;

    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;


    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    private Vector3 _previousPosition; // Vị trí ở frame trước
    private float _totalDistanceMoved = 0f; // Tổng quãng đường đã đi

    public bool IsAtShootingWaypoint { get; private set; } = false;
    private const int mana = 12;
    private Character character;
    private float duration = 0.05f;
    private float speedBoost = 3f;
    private float baseMoveSpeed;
    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;

    private void OnEnable()
    {
        baseMoveSpeed = moveSpeed;
        baseMoveSpeed += speedBoost;
        StartCoroutine(SetTrue());
        StartCoroutine(ReturnSpeadEnemy(duration));
    }
    private void OnDisable()
    {
        StopCoroutine(SetTrue());
    }

    void Start()
    {

        currentHealth = maxHealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        // Lấy tất cả các con của patrolPathParent và thêm chúng vào danh sách waypoint
        if (patrolPathParent != null)
        {
            foreach (Transform child in patrolPathParent)
            {
                waypoints.Add(child);
            }
        }

        if (waypoints.Count == 0)
        {
            Debug.LogWarning("Không tìm thấy điểm waypoint nào cho enemy " + gameObject.name + ". Đảm bảo có patrolPathParent và các con của nó.");
            gameObject.SetActive(false); // Tắt enemy nếu không có waypoint
            return;
        }
        _previousPosition = transform.position;

    }

    // void Update()
    // {
    //     MoveEnemy();
    // }
    void FixedUpdate()
    {
        if (waypoints.Count == 0) return;

        // Lấy vị trí mục tiêu
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        // Tính toán hướng di chuyển
        // Debug.Log($"currentWaypointIndex:{currentWaypointIndex}");
        Vector2 direction = (targetPosition - rigidbody2D.position).normalized;

        // Gán vận tốc cho Rigidbody2D
        rigidbody2D.velocity = direction * baseMoveSpeed;

        // Cập nhật tổng khoảng cách đã di chuyển
        float distanceThisFrame = Vector2.Distance(rigidbody2D.position, _previousPosition);
        _totalDistanceMoved += distanceThisFrame;
        _previousPosition = rigidbody2D.position;

        // Kiểm tra khoảng cách để chuyển waypoint
        if (Vector2.Distance(rigidbody2D.position, targetPosition) < arrivalThreshold)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
            }
        }
    }
    IEnumerator SetTrue()
    {
        yield return new WaitForSeconds(1.5f);
        IsAtShootingWaypoint = true;
    }
    IEnumerator ReturnSpeadEnemy(float time)
    {
        yield return new WaitForSeconds(time);
        baseMoveSpeed = moveSpeed;
    }
    private void MoveEnemy()
    {
        if (waypoints.Count == 0) return; // Bảo vệ khỏi lỗi nếu không có waypoint

        float distanceThisFrame = Vector3.Distance(transform.position, _previousPosition);
        _totalDistanceMoved += distanceThisFrame;

        _previousPosition = transform.position;

        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, baseMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
        {

            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0; // Lặp lại tuần tra
            }
        }
    }
    // private void MoveEnemy()
    // {
    //     if (waypoints.Count == 0) return;

    //     Vector3 targetPosition = waypoints[currentWaypointIndex].position;

    //     // Di chuyển bằng Rigidbody.MovePosition() thay vì transform.position
    //     Vector3 newPosition = Vector3.MoveTowards(rigidbody2D.position, targetPosition, baseMoveSpeed * Time.fixedDeltaTime);
    //     rigidbody2D.MovePosition(newPosition);

    //     // Kiểm tra khoảng cách
    //     if (Vector3.Distance(rigidbody2D.position, targetPosition) < arrivalThreshold)
    //     {
    //         currentWaypointIndex++;
    //         if (currentWaypointIndex >= waypoints.Count)
    //         {
    //             currentWaypointIndex = 0;
    //         }
    //     }
    // }

    void OnDrawGizmos()
    {
        if (patrolPathParent == null) return;

        // Để vẽ đường Gizmos, chúng ta cần lấy các waypoint từ parent
        List<Transform> gizmoWaypoints = new List<Transform>();
        foreach (Transform child in patrolPathParent)
        {
            gizmoWaypoints.Add(child);
        }

        if (gizmoWaypoints.Count == 0) return;

        Gizmos.color = Color.yellow;
        for (int i = 0; i < gizmoWaypoints.Count; i++)
        {
            Gizmos.DrawSphere(gizmoWaypoints[i].position, 0.2f);
            if (i < gizmoWaypoints.Count - 1)
            {
                Gizmos.DrawLine(gizmoWaypoints[i].position, gizmoWaypoints[i + 1].position);
            }
        }
        if (gizmoWaypoints.Count > 1) // Nối điểm cuối với điểm đầu nếu tuần tra lặp lại
        {
            Gizmos.DrawLine(gizmoWaypoints[gizmoWaypoints.Count - 1].position, gizmoWaypoints[0].position);
        }
    }


    public void TakeDamge(float damge)
    {
        currentHealth -= damge;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        ResetData();
        CharacterManager.Instance.Mana(mana);
        IsAtShootingWaypoint = false;
        this.gameObject.SetActive(false);

    }
    public void ResetData()
    {
        _totalDistanceMoved = 0f;
        _previousPosition = transform.position; // Đặt lại vị trí trước đó về vị trí hiện tại
        currentHealth = maxHealth;
        currentWaypointIndex = 0; // Đặt lại waypoint về đầu tiên

    }
    public float GetTotalDistanceTraveled()
    {
        return _totalDistanceMoved;
    }
    public float GetWaypointIndex()
    {
        Debug.Log($"currentWaypointIndex:{currentWaypointIndex}");
        return currentWaypointIndex;
    }

    public void SetUpMovement(float speed)
    {
        moveSpeed = speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bomb"))
        {
            Debug.Log("Va cham co ");

            collider2D.isTrigger = false;
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX |
                                   RigidbodyConstraints2D.FreezePositionY |
                                   RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(ActiveTrisgger());
        }
        if (other.CompareTag("EndGame"))
        {
            this.gameObject.SetActive(false);
            CharacterManager.Instance.TakeHealth(1);
        }
    }
    IEnumerator ActiveTrisgger()
    {
        yield return new WaitForSeconds(4f);
        collider2D.isTrigger = true;
        rigidbody2D.constraints = RigidbodyConstraints2D.None;
    }
}
