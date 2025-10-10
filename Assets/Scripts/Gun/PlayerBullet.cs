using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.CompareTag("Enemy"))
        {
            if (collision.collider.transform.TryGetComponent(out Entity entity))
            {
                entity.takeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
