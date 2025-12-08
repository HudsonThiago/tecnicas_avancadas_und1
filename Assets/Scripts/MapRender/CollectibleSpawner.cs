using UnityEngine;
using System.Collections;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Referências")]
    public PerlinTerrain terrain;
    public GameObject collectiblePrefab;

    [Header("Quantidade")]
    [Range(1, 200)]
    public int maxItems = 30;

    [Header("Máscara de densidade (Perlin)")]
    [Range(0.1f, 200f)]
    public float densityNoiseScale = 40f;

    [Range(0f, 1f)]
    public float spawnThreshold = 0.6f;

    public int densitySeed = 12345;

    [Header("Posicionamento")]
    public float raycastHeight = 50f;
    public float itemYOffset = 0.5f;

    [Header("Tentativas")]
    public int maxAttempts = 1000;

    [Header("Debug")]
    public bool debugLogs = true;

    private IEnumerator Start()
    {
        if (terrain == null || collectiblePrefab == null)
        {
            Debug.LogWarning("CollectibleSpawner: faltando referência de terrain ou prefab.");
            yield break;
        }

        MeshFilter mf = terrain.GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogWarning("CollectibleSpawner: PerlinTerrain sem MeshFilter.");
            yield break;
        }

        while (mf.sharedMesh == null)
        {
            if (debugLogs)
                Debug.Log("CollectibleSpawner: esperando terreno gerar a malha...");
            yield return null;
        }

        SpawnItems(mf.sharedMesh);
    }

    private void SpawnItems(Mesh mesh)
    {
        Bounds bounds = mesh.bounds;

        Vector3 terrainPos = terrain.transform.position;

        float minX = terrainPos.x + bounds.min.x;
        float maxX = terrainPos.x + bounds.max.x;
        float minZ = terrainPos.z + bounds.min.z;
        float maxZ = terrainPos.z + bounds.max.z;
        float rayY = terrainPos.y + bounds.max.y + raycastHeight;

        System.Random prng = new System.Random(densitySeed);
        float offsetX = prng.Next(-100000, 100000);
        float offsetZ = prng.Next(-100000, 100000);

        int spawned = 0;
        int attempts = 0;

        while (spawned < maxItems && attempts < maxAttempts)
        {
            attempts++;

            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);

            float sampleX = (x + offsetX) / densityNoiseScale;
            float sampleZ = (z + offsetZ) / densityNoiseScale;
            float noise = Mathf.PerlinNoise(sampleX, sampleZ);

            // controla densidade de itens
            if (noise < spawnThreshold)
                continue;

            Vector3 origin = new Vector3(x, rayY, z);

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
            {
                Vector3 spawnPos = hit.point + Vector3.up * itemYOffset;
                Instantiate(collectiblePrefab, spawnPos, Quaternion.identity, transform);
                spawned++;
            }
        }

        if (debugLogs)
            Debug.Log($"CollectibleSpawner: tentou {attempts} vezes e spawnou {spawned} itens.");
    }
}
