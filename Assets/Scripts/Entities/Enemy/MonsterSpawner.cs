using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyTier
    {
        [Tooltip("Prefab deste tipo de inimigo")]
        public GameObject prefab;

        [Tooltip("A partir de quantos inimigos spawnados este tipo começa a aparecer")]
        public int minSpawnCount = 0;
    }

    [Header("Referências")]
    [Tooltip("Terreno procedural gerado via PerlinTerrain")]
    public PerlinTerrain terrain;

    [Tooltip("Configuração dos tipos de inimigos e thresholds")]
    public EnemyTier[] enemyTiers;

    [Header("Pontos de spawn")]
    [Range(1, 20)]
    [Tooltip("Quantidade de pontos aleatórios gerados no mapa")]
    public int spawnPointCount = 4;

    [Header("Hordas / Respawn")]
    [Tooltip("Tempo entre spawns (em segundos)")]
    public float spawnInterval = 5f;

    [Tooltip("Quantos inimigos spawnam por horda")]
    public int enemiesPerWave = 4;

    [Header("Posicionamento")]
    [Tooltip("Altura do raycast acima do terreno")]
    public float raycastHeight = 50f;

    [Tooltip("Offset vertical para o inimigo não nascer cravado no chão")]
    public float enemyYOffset = 0.5f;

    [Tooltip("Raio de espalhamento em torno do ponto de spawn")]
    public float spreadRadius = 2f;

    [Header("Tentativas")]
    [Tooltip("Máximo de tentativas para encontrar pontos de spawn válidos")]
    public int maxAttemptsToFindPoints = 500;

    [Header("Debug")]
    public bool drawGizmos = true;
    public Color gizmoColor = Color.red;
    private readonly List<Vector3> spawnPoints = new List<Vector3>();
    private int totalSpawnCount = 0;
    private bool spawnerReady = false;


    // ----------------- CICLO DE VIDA -----------------

    private IEnumerator Start()
    {
        if (terrain == null || enemyTiers == null || enemyTiers.Length == 0)
        {
            Debug.LogWarning("EnemySpawner: faltando terrain ou enemyTiers.");
            yield break;
        }

        // garante que ao menos um tier tem prefab configurado
        bool anyPrefab = false;
        foreach (var tier in enemyTiers)
        {
            if (tier.prefab != null) { anyPrefab = true; break; }
        }
        if (!anyPrefab)
        {
            Debug.LogWarning("EnemySpawner: nenhum prefab configurado em enemyTiers.");
            yield break;
        }

        MeshFilter mf = terrain.GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogWarning("EnemySpawner: PerlinTerrain sem MeshFilter.");
            yield break;
        }

        // espera o PerlinTerrain gerar a malha
        while (mf.sharedMesh == null)
        {
            yield return null;
        }

        Mesh mesh = mf.sharedMesh;
        Bounds bounds = mesh.bounds;
        Vector3 terrainPos = terrain.transform.position;

        float minX = terrainPos.x + bounds.min.x;
        float maxX = terrainPos.x + bounds.max.x;
        float minZ = terrainPos.z + bounds.min.z;
        float maxZ = terrainPos.z + bounds.max.z;
        float rayY = terrainPos.y + bounds.max.y + raycastHeight;

        // 1) gerar spawnPoints válidos sobre o terreno
        int attempts = 0;
        spawnPoints.Clear();

        while (spawnPoints.Count < spawnPointCount && attempts < maxAttemptsToFindPoints)
        {
            attempts++;

            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            Vector3 origin = new Vector3(x, rayY, z);

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
            {
                Vector3 point = hit.point + Vector3.up * enemyYOffset;

                // evita pontos muito próximos uns dos outros
                bool tooClose = false;
                for (int i = 0; i < spawnPoints.Count; i++)
                {
                    if (Vector3.Distance(point, spawnPoints[i]) < spreadRadius * 2f)
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose) continue;

                spawnPoints.Add(point);
            }
        }

        Debug.Log($"EnemySpawner: gerou {spawnPoints.Count} pontos de spawn em {attempts} tentativas.");

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: nenhum ponto de spawn encontrado.");
            yield break;
        }

        spawnerReady = true;

        // 2) inicia loop de hordas
        StartCoroutine(SpawnLoop(rayY));
    }

    private IEnumerator SpawnLoop(float rayY)
    {
        while (spawnerReady)
        {
            yield return new WaitForSeconds(spawnInterval);

            // spawna uma "onda" de inimigos
            SpawnWave(rayY);
        }
    }


    // ----------------- LOGICA DE SPAWN -----------------

    private void SpawnWave(float rayY)
    {
        if (spawnPoints.Count == 0) return;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            // escolhe um dos pontos de spawn
            Vector3 basePoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // pequeno offset aleatório ao redor do ponto
            Vector2 offset2D = Random.insideUnitCircle * spreadRadius;
            Vector3 tentativePos = new Vector3(
                basePoint.x + offset2D.x,
                rayY,
                basePoint.z + offset2D.y
            );

            // ajusta para o chão
            if (Physics.Raycast(tentativePos, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
            {
                Vector3 finalPos = hit.point + Vector3.up * enemyYOffset;

                GameObject prefab = ChoosePrefabForCurrentDifficulty();
                if (prefab == null) continue;

                Instantiate(prefab, finalPos, Quaternion.identity, transform);
                totalSpawnCount++;
            }
        }
    }

    private GameObject ChoosePrefabForCurrentDifficulty()
    {
        // pega os tiers habilitados para o totalSpawnCount atual
        List<EnemyTier> available = new List<EnemyTier>();
        foreach (var tier in enemyTiers)
        {
            if (tier.prefab == null) continue;
            if (totalSpawnCount >= tier.minSpawnCount)
                available.Add(tier);
        }

        if (available.Count == 0)
        {
            // fallback: usa o primeiro tier com prefab
            foreach (var tier in enemyTiers)
            {
                if (tier.prefab != null)
                    return tier.prefab;
            }
            return null;
        }

        // escolhe aleatoriamente entre os tiers disponíveis
        EnemyTier chosen = available[Random.Range(0, available.Count)];
        return chosen.prefab;
    }
    }
