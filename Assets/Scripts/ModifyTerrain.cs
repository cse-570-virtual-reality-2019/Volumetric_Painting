using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ModifyTerrain : MonoBehaviour
{
    World world;
    GameObject cameraGO;
    public GameObject vive_controller_emulator;
    public bool useViveEmulator = false;

    void Start()
    {
        world = gameObject.GetComponent("World") as World;
        cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ReplaceBlockCursor(0);
        }

        if (Input.GetMouseButtonDown(1))
        {
            AddBlockCursor(1);
        }
    }

    public void ReplaceBlockCenter(float range, byte block)
    {
        //Replaces the block directly in front of the player
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                ReplaceBlockAt(hit, block);
            }
        }
    }

    public void AddBlockCenter(float range, byte block)
    {
        //Adds the block specified directly in front of the player
        Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                AddBlockAt(hit, block);
            }

            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
        }
    }

    public void ReplaceBlockCursor(byte block)
    {
        //Replaces the block specified where the mouse cursor is pointing
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            ReplaceBlockAt(hit, block);
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance),
                Color.green, 2);
        }
    }

    public void AddBlockCursor(byte block)
    {
        //Adds the block specified where the mouse cursor is pointing

        if (useViveEmulator)
        {
            AddBlockAt(vive_controller_emulator.transform.position, block);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                AddBlockAt(hit, block);
                Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance),
                    Color.green, 2);
            }
            else
            {
                AddBlockAt(ray.origin + (ray.direction * 4f), block);
                Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance),
                    Color.green, 2);
            }
        }
    }

    public void ReplaceBlockAt(RaycastHit hit, byte block)
    {
        //removes a block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
        Vector3 position = hit.point;
        position += (hit.normal * -0.5f);

        SetBlockAt(position, block);
    }

    public void AddBlockAt(RaycastHit hit, byte block)
    {
        //adds the specified block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
        Vector3 position = hit.point;
        position += (hit.normal * 0.5f);

        SetBlockAt(position, block);
    }

    public void AddBlockAt(Vector3 point, byte block)
    {
        Vector3 position = point;
        position += Vector3.up * 0.5f;

        SetBlockAt(position, block);
    }

    public void SetBlockAt(Vector3 position, byte block)
    {
        //sets the specified block at these coordinates
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z);

        SetBlockAt(x, y, z, block);
    }

    public void SetBlockAt(int x, int y, int z, byte block)
    {
        //adds the specified block at these coordinates
        print("Adding: " + x + ", " + y + ", " + z);


        world.data[x, y, z] = block;
        UpdateChunkAt(x, y, z);
    }

    public void UpdateChunkAt(int x, int y, int z)
    {
        //Updates the chunk containing this block
        int updateX = Mathf.FloorToInt(x / world.chunkSize);
        int updateY = Mathf.FloorToInt(y / world.chunkSize);
        int updateZ = Mathf.FloorToInt(z / world.chunkSize);

        print("Updating: " + updateX + ", " + updateY + ", " + updateZ);

//        world.chunks[updateX, updateY, updateZ].GenerateMesh();
        world.chunks[updateX, updateY, updateZ].update = true;

        if (x - (world.chunkSize * updateX) == 0 && updateX != 0)
        {
            world.chunks[updateX - 1, updateY, updateZ].update = true;
        }

        if (x - (world.chunkSize * updateX) == 15 && updateX != world.chunks.GetLength(0) - 1)
        {
            world.chunks[updateX + 1, updateY, updateZ].update = true;
        }

        if (y - (world.chunkSize * updateY) == 0 && updateY != 0)
        {
            world.chunks[updateX, updateY - 1, updateZ].update = true;
        }

        if (y - (world.chunkSize * updateY) == 15 && updateY != world.chunks.GetLength(1) - 1)
        {
            world.chunks[updateX, updateY + 1, updateZ].update = true;
        }

        if (z - (world.chunkSize * updateZ) == 0 && updateZ != 0)
        {
            world.chunks[updateX, updateY, updateZ - 1].update = true;
        }

        if (z - (world.chunkSize * updateZ) == 15 && updateZ != world.chunks.GetLength(2) - 1)
        {
            world.chunks[updateX, updateY, updateZ + 1].update = true;
        }
    }
}