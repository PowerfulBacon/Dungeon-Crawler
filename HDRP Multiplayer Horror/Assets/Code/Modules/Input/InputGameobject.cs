using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGameobject : MonoBehaviourPunCallbacks
{

    public Client owner;

    // Update is called once per frame
    void Update()
    {

        if (owner == null)
        {
            LogHandler.Log("Input manager attatched to no client.");
            Destroy(this);
            return;
        }

        if (!photonView.IsMine)
            return;

        owner.keyMap.SetAxisState("Horizontal", Input.GetAxis("Horizontal"));
        owner.keyMap.SetAxisState("Vertical", Input.GetAxis("Vertical"));
        owner.keyMap.SetAxisState("MouseX", Input.GetAxis("MouseX"));
        owner.keyMap.SetAxisState("MouseY", Input.GetAxis("MouseY"));
        owner.keyMap.SetKeyState("LeftMouse", Input.GetButton("LeftMouse"));
        owner.keyMap.SetKeyState("RightMouse", Input.GetButton("RightMouse"));
        owner.keyMap.SetKeyState("Space", Input.GetButton("Space"));
        owner.keyMap.SetKeyState("Tab", Input.GetButton("Tab"));

    }
}
