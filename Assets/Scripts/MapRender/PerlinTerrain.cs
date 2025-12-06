using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class PerlinTerrain : MonoBehaviour
{
    [Header("Tamanho do terreno")]
    public int width = 100;   // tamanho em X
    public int depth = 100;   // tamanho em Z

    [Header("Configuração do ruído")]
    [Range(0.1f, 100f)]
    public float noiseScale = 20f;   // escala do ruído (zoom)

    [Header("Altura")]
    [Range(0.01f, 10f)]
    public float heightMultiplier = 5f; // altura máxima do terreno

    [Header("Seed / Offset")]
    public int seed = 0;          // para mudar o mapa
    public Vector2 offset;        // offset manual

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Start()
    {
        GenerateTerrain();
    }

    [ContextMenu("Regerar Terreno")]
    public void GenerateTerrain()
    {
        Mesh mesh = new Mesh();
        mesh.name = "PerlinTerrainMesh";

        // 👇 PERMITE MAIS DE 65.535 ÍNDICES
        mesh.indexFormat = IndexFormat.UInt32;

        // --- VÉRTICES ---
        Vector3[] vertices = new Vector3[(width + 1) * (depth + 1)];

        System.Random prng = new System.Random(seed);
        Vector2 randomOffset = new Vector2(
            offset.x + prng.Next(-100000, 100000),
            offset.y + prng.Next(-100000, 100000)
        );

        if (noiseScale <= 0f) noiseScale = 0.0001f;

        int i = 0;
        for (int z = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                float sampleX = (x + randomOffset.x) / noiseScale;
                float sampleZ = (z + randomOffset.y) / noiseScale;

                float noiseValue = Mathf.PerlinNoise(sampleX, sampleZ); // 0..1
                float y = noiseValue * heightMultiplier;

                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        // --- TRIÂNGULOS ---
        int[] triangles = new int[width * depth * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;

                vert++;
                tris += 6;
            }
            vert++; // pula para a próxima linha de vértices
        }

        // --- APLICAR NA MALHA ---
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.sharedMesh = mesh;

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }
}
