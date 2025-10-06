using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyProjectile : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.parent.CompareTag("Player"))
        {
            if (collision.collider.transform.parent.TryGetComponent(out Entity entity))
            {
                entity.takeDamage(damage);
            }
        }
    }
}