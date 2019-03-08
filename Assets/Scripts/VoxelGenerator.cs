using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;

    public int xSize = 2;
    public int ySize = 2;
    public int zSize = 2;
    private int voxelVerticesSize;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        voxelVerticesSize = (xSize + 1) * (ySize + 1) * (zSize + 1);
        
        CreateShape();
        UpdateShape();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateShape()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
    }

    private void CreateShape()
    {
        vertices = new Vector3[voxelVerticesSize];

        int i = -1;
        for (int y = 0; y <= ySize; y++)
        for (int z = 0; z <= zSize; z++)
        for (int x = 0; x <= xSize; x++)
        {
            vertices[++i] = new Vector3(x, y, z);
            Debug.Log(vertices[i]);

        }
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        
        for (int i = 0; i < voxelVerticesSize; ++i)
        {
            Gizmos.DrawSphere(vertices[i], .1f);    
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
