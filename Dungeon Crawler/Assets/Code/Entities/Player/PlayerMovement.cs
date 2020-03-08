using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerModule
{

    public override void OnUpdate(Player parent)
    {

        if (!parent.photonView.IsMine)
            return;

        CharacterController characterController = parent.GetComponent<CharacterController>();

        Vector3 inputControlers = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        characterController.Move(inputControlers * Time.deltaTime);

    }

}
