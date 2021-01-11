using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : PlayerModule
{

    private Camera camera;

    public override void OnInitialise(Player parent)
    {
        camera = Object.FindObjectOfType<Camera>();
    }

    public override void OnUpdate(Player parent)
    {

        if (parent.GetVar<int>("health") <= 0)
            return;
        
        //If the mouse isn't locked don't update camera.
        if(!PlayerUserInterfaceModule.IsMouseLocked())
            return;

        //Put camera on player
        if (parent.GetComponentInChildren<Camera>() == null)
        {
            camera.transform.SetParent(parent.transform);
            camera.transform.localPosition = new Vector3(0, 0.6f, 0);
            camera.transform.localRotation = Quaternion.identity;
        }

        float MouseDeltaX = Input.GetAxis("Mouse X");
        float MouseDeltaY = Input.GetAxis("Mouse Y");

        parent.transform.Rotate(0, MouseDeltaX * Time.deltaTime * 50, 0);
        camera.transform.Rotate(MouseDeltaY * Time.deltaTime * -50, 0, 0);
    }

}
