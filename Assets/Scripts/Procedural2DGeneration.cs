﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural2DGeneration : MonoBehaviour
{
    // This first list contains every vertex of the mesh that we are going to render
    public List<Vector3> newVertices = new List<Vector3>();

    // The triangles tell Unity how to build each section of the mesh joining
    // the vertices
    public List<int> newTriangles = new List<int>();

    // The UV list is unimportant right now but it tells Unity how the texture is
    // aligned on each polygon
    public List<Vector2> newUV = new List<Vector2>();

    // To hold the type of block
    public byte[,] blocks;

    // Handle collider
    public List<Vector3> colVertices = new List<Vector3>();
    public List<int> colTriangles = new List<int>();


    // A mesh is made up of the vertices, triangles and UVs we are going to define,
    // after we make them up we'll save them as this mesh
    private Mesh mesh;
    private float tUnit = 0.25f;
    private Vector2 tStone = new Vector2(1, 0);
    private Vector2 tGrass = new Vector2(0, 1);
    private int squareCount; // To count which square are we rendering

    // Handle collider
    private int colCount;
    private MeshCollider col;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        GenTerrain();
        BuildMesh();
        UpdateMesh();
//        GenerateSquare(x, y);
    }

    private void GenerateSquare(int x, int y, Vector2 texture)
    {
        float z;
        newVertices.Add(new Vector3(x, y, 0));
        newVertices.Add(new Vector3(x + 1, y, 0));
        newVertices.Add(new Vector3(x + 1, y - 1, 0));
        newVertices.Add(new Vector3(x, y - 1, 0));

        newTriangles.Add(squareCount * 4);
        newTriangles.Add((squareCount * 4) + 1);
        newTriangles.Add((squareCount * 4) + 3);
        newTriangles.Add((squareCount * 4) + 1);
        newTriangles.Add((squareCount * 4) + 2);
        newTriangles.Add((squareCount * 4) + 3);

        newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y + tUnit));
        newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
        newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y));
        newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y));

        ++squareCount;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // User defined functions below this point

    void UpdateMesh()
    {
//        if (mesh == null) return;
        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
//        mesh.Optimize ();
        mesh.RecalculateNormals();

        // Handle mesgCoolider
        Mesh newMesh = new Mesh();
        newMesh.vertices = colVertices.ToArray();
        newMesh.triangles = colTriangles.ToArray();
        col.sharedMesh = newMesh;

        colVertices.Clear();
        colTriangles.Clear();
        colCount = 0;

        squareCount = 0;
        newVertices.Clear();
        newTriangles.Clear();
        newUV.Clear();
    }

    void GenerateCollider(int x, int y)
    {
//        Debug.Log(Block(x, y + 1));
//        Debug.Log(Block(x, y - 1));
//        Debug.Log(Block(x + 1, y));
//        Debug.Log(Block(x - 1, y));
        //Top
        if (Block(x, y + 1) == 0)
        {
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y, 0));
            colVertices.Add(new Vector3(x, y, 0));

            ColliderTriangles();
            ++colCount;
        }

        //bot
        if (Block(x, y - 1) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x, y - 1, 1));

            ColliderTriangles();
            ++colCount;
        }

        //left
        if (Block(x - 1, y) == 0)
        {
            colVertices.Add(new Vector3(x, y - 1, 1));
            colVertices.Add(new Vector3(x, y, 1));
            colVertices.Add(new Vector3(x, y, 0));
            colVertices.Add(new Vector3(x, y - 1, 0));

            ColliderTriangles();
            ++colCount;
        }

        //right
        if (Block(x + 1, y) == 0)
        {
            colVertices.Add(new Vector3(x + 1, y, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 1));
            colVertices.Add(new Vector3(x + 1, y - 1, 0));
            colVertices.Add(new Vector3(x + 1, y, 0));

            ColliderTriangles();
            ++colCount;
        }
    }

    void GenTerrain()
    {
        blocks = new byte[50, 30];

        for (int px = 0; px < blocks.GetLength(0); ++px)
        {
            int stone = Noise(px, 0, 5, 15, 1);
            stone+= Noise(px,0, 10,30,1);
//            stone+= Noise(px,0, 10,10,1);
//            stone+=75;
    
            int dirt = Noise(px,0, 20,35,1);
//            dirt+= Noise(px,0, 30,30,1);
//            dirt+=75;
            for (int py = 0; py < blocks.GetLength(1); ++py)
            {
                if (py < stone)
                {
                    blocks[px, py] = 1;
                } 
                else if (py < dirt)
                {
                    blocks[px, py] = 2;
                }
//                if (py == 5)
//                {
//                    blocks[px, py] = 2;
//                }
//                else if (py < 5)
//                {
//                    blocks[px, py] = 1;
//                }
            }
        }
    }

    void BuildMesh()
    {
        for (int px = 0; px < blocks.GetLength(0); ++px)
        {
            for (int py = 0; py < blocks.GetLength(1); ++py)
            {
                if (blocks[px, py] != 0)
                {
                    GenerateCollider(px, py);
                }

                if (blocks[px, py] == 1)
                {
                    GenerateSquare(px, py, tStone);
                }
                else if (blocks[px, py] == 2)
                {
                    GenerateSquare(px, py, tGrass);
                }
            }
        }
    }

    void ColliderTriangles()
    {
        colTriangles.Add(colCount * 4);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 3);
        colTriangles.Add((colCount * 4) + 1);
        colTriangles.Add((colCount * 4) + 2);
        colTriangles.Add((colCount * 4) + 3);
    }

    byte Block(int x, int y)
    {
        if (x == -1 || x == blocks.GetLength(0) || y == -1 || y == blocks.GetLength(1))
        {
            return (byte) 0;
        }

        return blocks[x, y];
    }

    int Noise(int x, int y, float scale, float mag, float exp)
    {
        return (int) (Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));
    }
}