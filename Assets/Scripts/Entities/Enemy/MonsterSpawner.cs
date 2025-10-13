using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    public int tempoInicial = 3;   // tempo antes do primeiro spawn
    public int interval = 5;       // intervalo entre ondas
    public int amount = 5;         // quantidade de monstros por onda
    public float radius = 10f;     // raio do spawn
    public GameObject prefab;      // prefab do monstro
    public Transform parent;       // objeto que será pai dos monstros instanciados

    [Header("Limite de monstros")]
    public int maxMonsters = 20;   // número máximo de monstros ativos na cena

    private float nextSpawnTime;
    private int currentMonsters = 0; // contador de monstros ativos

    void Start()
    {
        nextSpawnTime = Time.time + tempoInicial;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnWave();
            nextSpawnTime = Time.time + interval;
        }
    }

    void SpawnWave()
    {
        if (prefab == null)
        {
            return;
        }

        if (currentMonsters >= maxMonsters)
        {
            return;
        }

        int spawnCount = Mathf.Min(amount, maxMonsters - currentMonsters);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * radius;
            Vector3 spawnPos = new Vector3(
                transform.position.x + randomPos.x,
                1.9f,
                transform.position.z + randomPos.y
            );

            GameObject monster = Instantiate(prefab, spawnPos, Quaternion.identity, parent);

            currentMonsters++;

            MonsterLifeTracker tracker = monster.AddComponent<MonsterLifeTracker>();
            tracker.spawner = this;
        }
    }

    public void OnMonsterDestroyed()
    {
        currentMonsters = Mathf.Max(0, currentMonsters - 1);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        int segments = 40;
        float angleStep = 360f / segments;
        Vector3 prevPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            Vector3 nextPoint = transform.position + new Vector3(x, 0.01f, z);

            if (i > 0)
                Gizmos.DrawLine(prevPoint, nextPoint);
            else
                firstPoint = nextPoint;

            prevPoint = nextPoint;
        }

        Gizmos.DrawLine(prevPoint, firstPoint);
    }
}

public class MonsterLifeTracker : MonoBehaviour
{
    [HideInInspector] public MonsterSpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
            spawner.OnMonsterDestroyed();
    }
}