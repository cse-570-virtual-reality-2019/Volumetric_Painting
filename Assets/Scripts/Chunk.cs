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

    public GameObject worldGO;
    private World world;
    public int chunkSize = 16;

    // Start is called before the first frame update
    void Start()
    {
        world = worldGO.GetComponent("World") as World;
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        GenerateMesh();
//        CubeTop(0, 0, 0, 0);
//        CubeNorth(0, 0, 0, 0);
//        CubeEast(0, 0, 0, 0);
//        CubeSouth(0, 0, 0, 0);
//        CubeWest(0, 0, 0, 0);
//        CubeBot(0, 0, 0, 0);
//        UpdateMesh();
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
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));


        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);
    }

    void CubeWest(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y - 1, z));

        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);
    }

    void CubeBot(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));


        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);
    }

    void Cube(Vector2 texturePos)
    {
        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 1); //2
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4 + 3); //4

        newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
        newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
        newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
        newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y));

        ++faceCount; // Add this line
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
//        mesh.Optimize ();
        mesh.RecalculateNormals();

        col.sharedMesh = null;
        col.sharedMesh = mesh;

        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();

        faceCount = 0;
    }

    void GenerateMesh()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    //This code will run for every block in the chunk

                    if (world.Block(x, y, z) != 0)
                    {
                        //If the block is solid

                        if (world.Block(x, y + 1, z) == 0)
                        {
                            //Block above is air
                            CubeTop(x, y, z, world.Block(x, y, z));
                        }

                        if (world.Block(x, y - 1, z) == 0)
                        {
                            //Block below is air
                            CubeBot(x, y, z, world.Block(x, y, z));
                        }

                        if (world.Block(x + 1, y, z) == 0)
                        {
                            //Block east is air
                            CubeEast(x, y, z, world.Block(x, y, z));
                        }

                        if (world.Block(x - 1, y, z) == 0)
                        {
                            //Block west is air
                            CubeWest(x, y, z, world.Block(x, y, z));
                        }

                        if (world.Block(x, y, z + 1) == 0)
                        {
                            //Block north is air
                            CubeNorth(x, y, z, world.Block(x, y, z));
                        }

                        if (world.Block(x, y, z - 1) == 0)
                        {
                            //Block south is air
                            CubeSouth(x, y, z, world.Block(x, y, z));
                        }
                    }
                }
            }
        }

        UpdateMesh();
    }
}