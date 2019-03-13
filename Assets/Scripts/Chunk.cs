using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();

    private float tUnit = 0.25f;
    private Vector2 tStone = new Vector2(1, 0);
    private Vector2 tGrass = new Vector2(0, 1);

    private Mesh mesh;
    private MeshCollider col;

    private int faceCount;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        CubeTop(0, 0, 0, 0);
        CubeNorth(0, 0, 0, 0);
        CubeEast(0, 0, 0, 0);
        CubeSouth(0, 0, 0, 0);
        CubeWest(0, 0, 0, 0);
        CubeBot(0, 0, 0, 0);
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
    }


    // User defined functions below this
    void CubeTop(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x, y, z));

        
        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);
    }
    
    void CubeNorth(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

        
        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);
    }
    
    void CubeEast(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));

        
        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);
    }
    
    void CubeSouth(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3 (x, y - 1, z));
        newVertices.Add(new Vector3 (x, y, z));
        newVertices.Add(new Vector3 (x + 1, y, z));
        newVertices.Add(new Vector3 (x + 1, y - 1, z));

        
        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);
    }
    
    void CubeWest(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3 (x, y- 1, z + 1));
        newVertices.Add(new Vector3 (x, y, z + 1));
        newVertices.Add(new Vector3 (x, y, z));
        newVertices.Add(new Vector3 (x, y - 1, z));
        
        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);
    }
    
    void CubeBot(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3 (x,  y-1,  z ));
        newVertices.Add(new Vector3 (x + 1, y-1,  z ));
        newVertices.Add(new Vector3 (x + 1, y-1,  z + 1));
        newVertices.Add(new Vector3 (x,  y-1,  z + 1));

        
        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);
    }

    void Cube(Vector2 texturePos)
    {
        newTriangles.Add(faceCount * 4  ); //1
        newTriangles.Add(faceCount * 4 + 1 ); //2
        newTriangles.Add(faceCount * 4 + 2 ); //3
        newTriangles.Add(faceCount * 4  ); //1
        newTriangles.Add(faceCount * 4 + 2 ); //3
        newTriangles.Add(faceCount * 4 + 3 ); //4
  
        newUV.Add(new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
        newUV.Add(new Vector2 (tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
        newUV.Add(new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
        newUV.Add(new Vector2 (tUnit * texturePos.x, tUnit * texturePos.y));
  
        ++faceCount; // Add this line
    }

    void UpdateMesh()
    {
        mesh.Clear ();
        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
//        mesh.Optimize ();
        mesh.RecalculateNormals ();
  
  
        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();
  
        faceCount=0;
    }
}