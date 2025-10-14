using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.parent.CompareTag("Player"))
        {
            if (collider.transform.parent.TryGetComponent(out Entity entity))
            {
                entity.takeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}