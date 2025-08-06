using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] float damge;
    private Character character;
    private void Start()
    {
        character = FindObjectOfType<Character>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var damageCharacter = character.ReturnDamage();
            other.GetComponent<Enemy>().TakeDamge(damageCharacter);
            this.gameObject.SetActive(false);
        }
        if (other.CompareTag("EnemyAI"))
        {
            other.GetComponent<EnemyAI>().TakeDamge(damge);
            this.gameObject.SetActive(false);
        }
    }
}
