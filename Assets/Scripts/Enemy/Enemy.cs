using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{


    [Header("Patrol Settings")]
    [Tooltip("Kéo GameObject cha chứa các điểm waypoint vào đây. Các waypoint con của nó sẽ được sử dụng.")]
    public Transform patrolPathParent; // Tham chiếu đến GameObject cha chứa các waypoint

    [Tooltip("Tốc độ di chuyển của enemy.")]
    public float moveSpeed = 3f;

    [Tooltip("Khoảng cách tối thiểu để coi là đã đến điểm waypoint.")]
    public float arrivalThreshold = 0.1f;

    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypointIndex = 0;


    [SerializeField] float maxHealth = 100f;
    float currentHealth;

    private Vector3 _previousPosition; // Vị trí ở frame trước
    private float _totalDistanceMoved = 0f; // Tổng quãng đường đã đi

    public bool IsAtShootingWaypoint { get; private set; } = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShootingZone"))
        {
            IsAtShootingWaypoint = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ShootingZone"))
        {
            IsAtShootingWaypoint = true;
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
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

    void Update()
    {

        if (waypoints.Count == 0) return; // Bảo vệ khỏi lỗi nếu không có waypoint

        float distanceThisFrame = Vector3.Distance(transform.position, _previousPosition);
        _totalDistanceMoved += distanceThisFrame;
        _previousPosition = transform.position;

        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < arrivalThreshold)
        {

            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0; // Lặp lại tuần tra
            }
        }
    }


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
        Debug.Log($"currentHealth1:{currentHealth}");

        currentHealth -= damge;
        Debug.Log($"currentHealth:{currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        ResetData();
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
}
