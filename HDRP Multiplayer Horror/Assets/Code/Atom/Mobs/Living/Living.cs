using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Living : Mob
{

    public bool hasHeadRotation = false;

    public float angleClamp = 60;
    public float speed; //Speed modifier of the mob

    public int health;  //Overall health of this mob

    public Vector3 recordedPosition;    //For calculating velocity clientside
    public Vector3 currentVelocity; //Current velocity of the mob

    public CharacterController characterController;

    public override void Initialize()
    {
        base.Initialize();

        //Locate our character controller
        characterController = GetComponent<CharacterController>();
    }

    public override void LocalUpdate()
    {
        base.LocalUpdate();
        Vector3 deltaPosition = transform.position - recordedPosition;
        currentVelocity = deltaPosition / Time.deltaTime;
        recordedPosition = transform.position;
    }

    public override void Life()
    {
        base.Life();
        CheckDeath();
    }

    public override void ClientLife()
    {
        base.ClientLife();
        HandleMovement();
    }

    /// <summary>
    /// Handles control based movement
    /// </summary>
    public virtual void HandleMovement()
    {
        //=============================
        //Handle rotational movement
        //=============================
        if(CanLook())
        {
            Vector2 inputMap = new Vector2();
            inputMap.x = client.keyMap.GetAxisState("MouseX") * client.prefs.sensitivity * Time.deltaTime;
            inputMap.y = client.keyMap.GetAxisState("MouseY") * client.prefs.sensitivity * Time.deltaTime;

            if(inputMap.magnitude != 0)
            {
                transform.Rotate(0, inputMap.x, 0, Space.Self);
            }

            if (hasHeadRotation)
            {
                float yRotation = inputMap.y;

                cameraAttachmentPoint.transform.Rotate(inputMap.y, 0, 0, Space.Self);
                Vector3 angleAfter = cameraAttachmentPoint.transform.localRotation.eulerAngles;
                angleAfter.x = Mathf.Clamp(((angleAfter.x + 180) % 360) - 180, -angleClamp, angleClamp);
                cameraAttachmentPoint.transform.localRotation = Quaternion.Euler(angleAfter);
            }
            else
            {
                //Todo handle camera rotation urgh
            }

        }

        //=============================
        //Handle translational movement
        //=============================
        if (CanMove())
        {
            Vector2 inputMap = new Vector2();
            inputMap.x = client.keyMap.GetAxisState("Horizontal");
            inputMap.y = client.keyMap.GetAxisState("Vertical");

            if (inputMap.magnitude != 0)
            {
                //Set the magnitude of the input map to 2 so you can't move faster by going sideways and forward at the same time
                inputMap.SqrMagnitude();

                //Multiply by speed
                inputMap *= speed * Time.deltaTime;

                //Move <3
                characterController.Move(inputMap.y * transform.forward + inputMap.x * transform.right);
            }
        }

    }

    /// <summary>
    /// Overridable function that defines if the mob can move.
    /// Think 173 not being able to move while in observation.
    /// </summary>
    /// <returns></returns>
    public virtual bool CanMove()
    {
        if (speed == 0)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Overrideable function that defines if the mob can look around
    /// </summary>
    /// <returns></returns>
    public virtual bool CanLook()
    {
        return true;
    }

    /// <summary>
    /// Checks if the mob is dead and spawns an observer mob if it is.
    /// </summary>
    public virtual void CheckDeath()
    {

    }

}
