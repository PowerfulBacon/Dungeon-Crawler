using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : PlayerModule
{

    public override void OnUpdate(Player parent)
    {

        if (parent.health <= 0)
            return;

        Cursor.lockState = CursorLockMode.Locked;

        //Put camera on player
        if (parent.GetComponentInChildren<Camera>() == null)
        {
            Object.FindObjectOfType<Camera>().transform.SetParent(parent.transform);
            Object.FindObjectOfType<Camera>().transform.localPosition = new Vector3(0, 0.6f, 0);
            Object.FindObjectOfType<Camera>().transform.localRotation = Quaternion.identity;
        }

        float MouseDeltaX = Input.GetAxis("Mouse X");
        float MouseDeltaY = Input.GetAxis("Mouse Y");

        parent.transform.Rotate(0, MouseDeltaX * Time.deltaTime * 50, 0);
        Object.FindObjectOfType<Camera>().transform.Rotate(MouseDeltaY * Time.deltaTime * -50, 0, 0);
    }

}
