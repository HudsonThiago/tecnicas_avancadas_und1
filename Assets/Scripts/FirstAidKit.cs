using UnityEngine;

public class FirstAidKit : MonoBehaviour
{
    public float health = 50f;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.parent.CompareTag("Player"))
        {
            if (collider.transform.parent.TryGetComponent(out Entity entity))
            {
                entity.getHealth(health);
                gameObject.SetActive(false);
            }
        }
    }
}