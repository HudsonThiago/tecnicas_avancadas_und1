using UnityEngine;
using System.Collections;

public class SpawnAtTerrainCenter : MonoBehaviour
{
    [SerializeField] private PerlinTerrain terrain; // arrasta no Inspector se quiser
    [SerializeField] private float extraHeight = 3f; // quanto acima do chão o player nasce

    private IEnumerator Start()
    {
        // Se não tiver arrastado no Inspector, tenta achar na cena
        if (terrain == null)
        {
#if UNITY_6000_0_OR_NEWER
            terrain = Object.FindFirstObjectByType<PerlinTerrain>();
#else
            terrain = Object.FindObjectOfType<PerlinTerrain>();
#endif
        }

        if (terrain == null)
        {
            Debug.LogWarning("SpawnAtTerrainCenter: nenhum PerlinTerrain encontrado na cena.");
            yield break;
        }

        MeshFilter mf = terrain.GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogWarning("SpawnAtTerrainCenter: PerlinTerrain não tem MeshFilter.");
            yield break;
        }

        // Espera até a mesh existir (caso o terreno gere no Start)
        while (mf.sharedMesh == null)
        {
            yield return null; // espera 1 frame
        }

        Mesh mesh = mf.sharedMesh;
        Bounds bounds = mesh.bounds;

        // Centro da malha em coordenadas locais
        Vector3 localCenter = bounds.center;

        // Converte para mundo
        Vector3 worldCenter = terrain.transform.TransformPoint(localCenter);

        // Altura: centro + metade da altura do terreno + extraHeight
        float height = worldCenter.y + bounds.extents.y + extraHeight;

        transform.position = new Vector3(worldCenter.x, height, worldCenter.z);
    }
}
