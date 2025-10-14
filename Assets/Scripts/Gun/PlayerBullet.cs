using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Enemy"))
        {
            if (collider.transform.TryGetComponent(out Entity entity))
            {
                entity.takeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
