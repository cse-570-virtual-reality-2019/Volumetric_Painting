using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Valve.VR;

public class ViveInputTest : MonoBehaviour
{
    // [SteamVR_DefaultAction("Squueze")]
    public SteamVR_Action_Single squeezeAction;
    public SteamVR_Action_Vector2 touchPadAction;
    public GameObject controllerRight;
    public GameObject controllerLeft;
    public GameObject cameraRig;
    public GameObject camera;
    public GameObject world;
    public GameObject canvas;
    public GameObject pointer;
    public float speed = 2;
    public float offset = 2;
    public float buttonRate;
    private float nextButtonPress;
    public Vector2 texture = new Vector2(0,0);
    public GameObject voxelCar;
    public GameObject voxelPig;
    private bool voxelCarPresent = false;
    private bool voxelPigPresent = false;
    
    // Start is called before the first frame update
    void Start () {  } 
   

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraForward = camera.gameObject.transform.forward;
        Vector3 cameraRight = new Vector3(cameraForward.z, cameraForward.y, -cameraForward.x);
        Vector3 cameraLeft = -cameraRight;
        /*if (SteamVR_Actions._default.Teleport.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            print("Teleport down");
        }
        if (SteamVR_Actions._default.GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            print("Grab Pinch up");
        }*/

        float triggerValueLeft = squeezeAction.GetAxis((SteamVR_Input_Sources.LeftHand));
        float triggerValueRight = squeezeAction.GetAxis((SteamVR_Input_Sources.RightHand));
        
        
        //print(SteamVR_Actions._default.up.state);
        
        if (triggerValueRight == 1)
        {
            Vector3 position = controllerRight.gameObject.transform.position;
            position += cameraForward * offset;
            world.GetComponent<ModifyTerrain>().AddBlockAt(position, 1);
            var x = Mathf.RoundToInt(position.x) / world.GetComponent<World>().chunkSize;
            var y = Mathf.RoundToInt(position.y) / world.GetComponent<World>().chunkSize;
            var z = Mathf.RoundToInt(position.z) / world.GetComponent<World>().chunkSize;
            world.GetComponent<World>().chunks[x, y, z].tStone = texture;
            world.GetComponent<World>().chunks[x, y, z].tGrass = texture;
            world.GetComponent<World>().chunks[x, y, z].tGrassTop = texture;
            
        }

        if (triggerValueLeft == 1)
        {
            Vector3 position = controllerLeft.gameObject.transform.position;
            position += cameraForward * offset;
            world.GetComponent<ModifyTerrain>().SetBlockAt(position, 0);

        }

        //print(SteamVR_Actions._default.up.GetState(SteamVR_Input_Sources.RightHand));

        if (SteamVR_Actions._default.up.GetState(SteamVR_Input_Sources.RightHand) == true) 
        {
            //print("Moving forward");
            cameraRig.transform.position += cameraForward * Time.deltaTime * speed;
        }
        
        if (SteamVR_Actions._default.down.GetState(SteamVR_Input_Sources.RightHand) == true) 
        {
            cameraRig.transform.position +=  (- cameraForward) * Time.deltaTime * speed;
        }
        
        if (SteamVR_Actions._default.left.GetState(SteamVR_Input_Sources.RightHand) == true) 
        {
            cameraRig.transform.position += cameraLeft * Time.deltaTime * speed;
        }
        
        if (SteamVR_Actions._default.right.GetState(SteamVR_Input_Sources.RightHand) == true) 
        {
            cameraRig.transform.position += cameraRight * Time.deltaTime * speed;
        }


        if (SteamVR_Actions._default.colorSelector.GetState(SteamVR_Input_Sources.RightHand) && Time.time > nextButtonPress)
        {
            print("Center trackpad pressed");
            canvas.SetActive(!canvas.activeInHierarchy);
            pointer.SetActive(!pointer.activeInHierarchy);
            nextButtonPress = Time.time + buttonRate;
        }


        /*Vector2 touchpadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);

        if (touchpadValue != Vector2.zero)
        {
            print(touchpadValue);
        }*/
    }
    
    public void ButtonClickHandler(int position)
    {
        if (position == 0)
        {
            // Change texture to 0
            texture = new Vector2(3,3);
        }
        else if (position == 1)
        {
            texture = new Vector2(1,1);
            // Change texture to 1
        }
        else if (position == 2)
        {
            Vector3 cameraForward = camera.gameObject.transform.forward;
            Vector3 location = controllerRight.gameObject.transform.position;
            location += cameraForward * offset * 4;
            voxelCar.transform.position = new Vector3(location.x, voxelCar.transform.position.y, location.z);
            voxelCar.SetActive(!voxelCar.activeInHierarchy);

        }
        else if (position == 3)
        {
            Vector3 cameraForward = camera.gameObject.transform.forward;
            Vector3 location = controllerRight.gameObject.transform.position;
            location += cameraForward * offset * 4;
            voxelPig.transform.position = new Vector3(location.x, voxelPig.transform.position.y, location.z);
            voxelPig.SetActive(!voxelPig.activeInHierarchy);

        } 
        else if (position == 4)
        {
            // Store xyz of pig and car in a file.
            //print("Saving");
            var positions = new StringBuilder();
            positions.Append(voxelCar.transform.position.x + "," + 
                             voxelCar.transform.position.y + "," +
                             voxelCar.transform.position.z + ";" + 
                             voxelCar.activeInHierarchy + "\n");
            
            positions.Append(voxelPig.transform.position.x + "," +
                             voxelPig.transform.position.y + "," +
                             voxelPig.transform.position.z + ";" + 
                             voxelPig.activeInHierarchy + "\n");
            File.WriteAllText(Application.dataPath + "/saveState.csv", positions.ToString());
        } 
        else if (position == 5)
        {
            string[] lines = File.ReadAllLines(Application.dataPath + "/saveState.csv");
            string[] voxelCarPosition = lines[0].Split(';');
            string[] locationVoxelCar = voxelCarPosition[0].Split(',');
            voxelCar.transform.position = new Vector3(float.Parse(locationVoxelCar[0]), float.Parse(locationVoxelCar[1]), float.Parse(locationVoxelCar[2]));
            voxelCar.SetActive(bool.Parse(voxelCarPosition[1]));
            
            string[] voxelPigPosition = lines[1].Split(';');
            string[] locationVoxelPig = voxelPigPosition[0].Split(',');
            voxelPig.transform.position = new Vector3(float.Parse(locationVoxelPig[0]), float.Parse(locationVoxelPig[1]), float.Parse(locationVoxelPig[2]));
            voxelPig.SetActive(bool.Parse(voxelPigPosition[1]));

        } 
    }
}

