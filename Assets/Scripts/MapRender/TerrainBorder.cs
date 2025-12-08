using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class TerrainBorders : MonoBehaviour
{
    [Header("Configuração da borda")]
    public float wallHeight = 10f;
    public float wallThickness = 2f;
    public Material wallMaterial;

    private IEnumerator Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();

        while (mf == null || mf.sharedMesh == null)
        {
            yield return null;
            mf = GetComponent<MeshFilter>();
        }

        Mesh mesh = mf.sharedMesh;
        Bounds bounds = mesh.bounds;

        float sizeX = bounds.size.x;
        float sizeZ = bounds.size.z;

        CreateWall(
            localPos: new Vector3(bounds.center.x, wallHeight / 2f, bounds.max.z + wallThickness / 2f),
            size:     new Vector3(sizeX, wallHeight, wallThickness)
        );

        CreateWall(
            localPos: new Vector3(bounds.center.x, wallHeight / 2f, bounds.min.z - wallThickness / 2f),
            size:     new Vector3(sizeX, wallHeight, wallThickness)
        );

        CreateWall(
            localPos: new Vector3(bounds.max.x + wallThickness / 2f, wallHeight / 2f, bounds.center.z),
            size:     new Vector3(wallThickness, wallHeight, sizeZ)
        );

        CreateWall(
            localPos: new Vector3(bounds.min.x - wallThickness / 2f, wallHeight / 2f, bounds.center.z),
            size:     new Vector3(wallThickness, wallHeight, sizeZ)
        );
    }

    private void CreateWall(Vector3 localPos, Vector3 size)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "BorderWall";
        wall.transform.SetParent(transform, false);
        wall.transform.localPosition = localPos;
        wall.transform.localScale = size;

        if (wallMaterial != null)
        {
            var rend = wall.GetComponent<Renderer>();
            if (rend != null)
                rend.material = wallMaterial;
        }
    }
}
