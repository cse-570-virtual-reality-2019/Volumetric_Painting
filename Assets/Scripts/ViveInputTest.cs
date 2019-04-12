using System.Collections;
using System.Collections.Generic;
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
    public float speed = 2;
    public float offset = 2;
    
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

        /*Vector2 touchpadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);

        if (touchpadValue != Vector2.zero)
        {
            print(touchpadValue);
        }*/
    }
}
