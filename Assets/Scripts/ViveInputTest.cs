using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInputTest : MonoBehaviour
{
    // [SteamVR_DefaultAction("Squueze")]
    public SteamVR_Action_Single squeezeAction;
    public SteamVR_Action_Vector2 touchPadAction;
    public SteamVR_TrackedObject trackedObject;
    public GameObject controllerRight;
    public GameObject controllerLeft;
    public GameObject cameraRig;
    public GameObject world;
    
    // Start is called before the first frame update
    void Start () { trackedObject = GetComponent<SteamVR_TrackedObject>(); } 
   

    // Update is called once per frame
    void Update()
    {
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

        
        if (triggerValueRight == 1)
        {
            Vector3 position = controllerRight.gameObject.transform.position;
            world.GetComponent<ModifyTerrain>().AddBlockAt(position, 1);
            
            print("Right: " + position);
        }

        if (triggerValueLeft == 1)
        {
            Vector3 position = controllerLeft.gameObject.transform.position;
            world.GetComponent<ModifyTerrain>().SetBlockAt(position, 0);
            print("Left: " + position);

        }

        Vector2 touchpadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);

        if (touchpadValue != Vector2.zero)
        {
            print(touchpadValue);
        }
    }
}
