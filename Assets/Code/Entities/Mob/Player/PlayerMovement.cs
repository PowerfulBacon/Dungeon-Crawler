using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerModule
{

    public override void OnUpdate(Player parent)
    {

        if (!parent.photonView.IsMine)
            return;

        //Slow!
        CharacterController characterController = parent.GetComponent<CharacterController>();

        Vector3 inputControlers = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3 outputMovement = new Vector3();
        outputMovement += parent.transform.forward * inputControlers.z * parent.stats.GetSpeed(parent);
        outputMovement += parent.transform.right * inputControlers.x * parent.stats.GetSpeed(parent);

        characterController.Move(outputMovement * Time.deltaTime);

    }

}
