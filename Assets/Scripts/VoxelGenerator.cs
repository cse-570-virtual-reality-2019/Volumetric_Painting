using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGenerator : MonoBehaviour
{
    // This first list contains every vertex of the mesh that we are going to render
    public List<Vector3> newVertices = new List<Vector3>();
    
    // The triangles tell Unity how to build each section of the mesh joining
    // the vertices
    public List<int> newTriangles = new List<int>();
    
    // The UV list is unimportant right now but it tells Unity how the texture is
    // aligned on each polygon
    public List<Vector2> newUV = new List<Vector2>();
    
    
    // A mesh is made up of the vertices, triangles and UVs we are going to define,
    // after we make them up we'll save them as this mesh
    private Mesh mesh;
    private float tUnit = 0.25f;
    private Vector2 tStone = new Vector2 (0, 0);
    private Vector2 tGrass = new Vector2 (0, 1);

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter> ().mesh;
   
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
   
   
        newVertices.Add( new Vector3 (x  , y  , z ));
        newVertices.Add( new Vector3 (x + 1 , y  , z ));
        newVertices.Add( new Vector3 (x + 1 , y-1  , z ));
        newVertices.Add( new Vector3 (x  , y-1  , z ));
   
        newTriangles.Add(0);
        newTriangles.Add(1);
        newTriangles.Add(3);
        newTriangles.Add(1);
        newTriangles.Add(2);
        newTriangles.Add(3);
        
        newUV.Add(new Vector2 (tUnit * tStone.x, tUnit * tStone.y + tUnit));
        newUV.Add(new Vector2 (tUnit * tStone.x + tUnit, tUnit * tStone.y + tUnit));
        newUV.Add(new Vector2 (tUnit * tStone.x + tUnit, tUnit * tStone.y));
        newUV.Add(new Vector2 (tUnit * tStone.x, tUnit * tStone.y));
   
        
    }

    // Update is called once per frame
    void Update()
    {
        mesh.Clear ();
        mesh.vertices = newVertices.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.uv = newUV.ToArray();
//        mesh.Optimize ();
        mesh.RecalculateNormals ();
    }
}