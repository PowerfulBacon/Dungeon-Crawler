using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerInteraction : PlayerModule
{

    private float nextInteractionWorldItem;
    private Camera camera;

    public override void OnInitialise(Player parent)
    {
        camera = Object.FindObjectOfType<Camera>();
    }

    public override void OnUpdate(Player parent)
    {

        if (parent.GetVar<int>("health") <= 0)
            return;

        if(Input.GetButtonDown("interaction"))
        {
            //Raycast
            RaycastHit hit;
            int layermask = 1 << LayerMask.NameToLayer("GroundItem");
            if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 3.0f, layermask))
            {
                //Check what we hit
                GameObject hitObject = hit.collider.gameObject;
                Item item = hitObject.GetComponent<Item>();

                if(item == null)
                {
                    return;
                }

                parent.ToChat($"<style=notice>That's a {item.itemName}.</style>");

                item.PickupItem();
            }
        }

        if(Input.GetButtonDown("Fire1"))
        {
            if(nextInteractionWorldItem < Time.time)
            {
                //Get held item
                Item item = parent.heldItem;
                //Use the item (if it exists)
                if(item != null)
                {
                    Log.PrintDebug($"Attacking using {item.ToString()}");
                    item.StartAttack(parent);
                }
                //Activate use delay
                nextInteractionWorldItem = Time.time + parent.GetUseDelay();
            }
        }

    }

}
