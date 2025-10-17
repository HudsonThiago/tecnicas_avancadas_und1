using UnityEngine;

public class EnemyDeathReward : MonoBehaviour
{
    [SerializeField] private int points = 100;
    private Entity entity;

    void Awake()
    {
        entity = GetComponent<Entity>(); // o mesmo Entity que você já usa
    }

    void OnDestroy()
    {
        // Só em Play Mode
        if (!Application.isPlaying) return;

        // Só conta se for um inimigo e se morreu (health <= 0)
        if (!CompareTag("Enemy")) return;
        if (entity == null || entity.health > 0f) return;

        // Acha o Player e soma os pontos
        if (GameObject.FindWithTag("Player") != null && GameObject.FindWithTag("Player").TryGetComponent(out Player player))
        {
            player.playerPoint += points;
        }
    }
}
