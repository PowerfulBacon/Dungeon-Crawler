using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anything that can be put in the inventory ---MUST--- inherit this :) <3
/// </summary>
public class Item : Entity
{

    public const float WORLD_SCALE = 1.0f;
    public const float EQUIPPED_SCALE = 0.5f;

    public static Material globalMaterial;

    public Sprite itemIcon;

    //If picked up mid physics
    public override void OnInitialise()
    {
        base.OnInitialise();
        AddVar("onGround", false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (GetVar<bool>("onGround"))
        {
            OnUpdateGround();
        }
        else
        {
            OnUpdateHeld();
        }
    }

    //Called every update when being held in hand
    public virtual void OnUpdateHeld()
    {

    }

    //Called every update when on the ground
    public virtual void OnUpdateGround()
    {

    }

    //When the item is put into a hand
    public virtual void OnEquip(Mob m)
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    //Called when the item is moved from hand to not hand
    public virtual void OnDequip(Mob m)
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    //Called when the item is picked up
    public virtual void OnPickup(Mob m)
    {
        if (!GetVar<bool>("onGround"))
            return;
        Log.Print("Attempted to pickup item");
        SetVar("onGround", false);
        GetComponent<MeshRenderer>().enabled = false;
    }

    //Called when item is used by holder (Inventory > Use)
    public virtual void OnUse(Mob m)
    {
    }

    //Called when item is dropped by holder
    public virtual void Drop(Mob m)
    {
        SetVar("onGround", true);
        GetComponent<MeshRenderer>().enabled = true;
    }

    //Called when item is thrown by holder
    public virtual void Throw(Mob m)
    {
        SetVar("onGround", true);
        GetComponent<MeshRenderer>().enabled = true;
    }


    //Used to convert the objects model scale into the world scale (used when on ground / in hand)
    public virtual void SetWorldScale()
    {

    }

    //Sets the model's scale to the scale used in the viewspace
    public virtual void SetViewSpaceScale()
    {

    }

}
