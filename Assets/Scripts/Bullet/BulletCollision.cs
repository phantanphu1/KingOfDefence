using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] float damge;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamge(damge);
            this.gameObject.SetActive(false);
        }
        if (other.CompareTag("EnemyAI"))
        {
            other.GetComponent<EnemyAI>().TakeDamge(damge);
            this.gameObject.SetActive(false);
        }
    }
}
