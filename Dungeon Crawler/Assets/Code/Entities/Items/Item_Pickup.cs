using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Code handling the picking up and dropping of entities in the inventory.
/// </summary>
public partial class Item : Entity
{

    //======== Pickup Code =========

    //When the item is put into a hand
    public virtual void OnEquip(Mob m)
    {
        GetComponent<MeshRenderer>().enabled = true;
        photonView.RPC("RPCClientEquip", RpcTarget.Others);
    }

    [PunRPC]
    public void RPCClientEquip()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    //Called when the item is moved from hand to not hand
    public virtual void OnDequip(Mob m)
    {
        //On the ground, dont disable mesh rendering.
        GetComponent<MeshRenderer>().enabled = false;
        photonView.RPC("RPCClientDequip", RpcTarget.Others);
    }

    [PunRPC]
    public void RPCClientDequip()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    /// <summary>
    /// Safe pickup method that handles picking up items.
    /// Different effects depending on client or server.
    /// Called on a client when they want to pick something up.
    /// </summary>
    public void PickupItem()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if(!photonView.IsMine)
            {
                Log.PrintError("We attempted to pickup an item, however we aren't controlling it. Request ignored");
                return;
            }
            photonView.RPC("RPCPickedUpAll", RpcTarget.All, Player.myPlayer.GetVar<int>("mobId"));
            PutIntoInventory();
        }
        else
        {
            //Sends a request to the server to pickup
            photonView.RPC("RPCServerTryPickup", Photon.Pun.RpcTarget.MasterClient, Player.myPlayer.GetVar<int>("mobId"));
        }
    }

    /// <summary>
    /// Request ran on the server, called by a client
    /// </summary>
    /// <param name="info"></param>
    [PunRPC]
    public void RPCServerTryPickup(int mobId, PhotonMessageInfo info)
    {
        if(!photonView.IsMine)
        {
            Log.PrintError("Someone attempted to pickup an item, however we aren't controlling it. Request ignored");
            return;
        }

        //We should check distance for an anti-cheat, not that I think at this point anyone will actually play this game let alone cheat in it ;^(

        //Enjoy your new home
        photonView.TransferOwnership(info.Sender);

        //Its been picked up, tell all clients to stop rendering it.
        photonView.RPC("RPCPickedUpAll", RpcTarget.All, mobId);
    }

    /// <summary>
    /// TODO: Make it so the item appears near the user picked up (as their child)
    /// </summary>
    [PunRPC]
    public void RPCPickedUpAll(int mobId, PhotonMessageInfo info)
    {
        Log.PrintDebug("Item was picked up, rendering disabled.");
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        //Set parent
        //Get the mob with ID mobId
        Mob caller = Mob.GetMobById(mobId);
        transform.SetParent(caller.transform);
        //Set local position
        transform.localPosition = new Vector3(0.4f, 0.2f, 0.4f);
        transform.localRotation = Quaternion.identity;
    }
    
    public override void GainOwnership()
    {
        Log.PrintDebug($"Ownership of {ToString()} transfered.");
        if(!photonView.IsMine)
        {
            Log.PrintError("I have no idea how this happened but you managed to recieve ownership or something you don't own. This is a photon networking issue I assume.");
            return;
        }
        if(GetVar<bool>("onGround"))
            //We are assuming we wanted this object, since we own it not we are going to force it into out inventory.
            PutIntoInventory();
        else
        {
            Log.PrintDebug("Putting item onto the ground.");
            //Assuming we are the server here.
            SetVar("onGround", true);
        }
    }

    private void PutIntoInventory()
    {
        if(Player.myPlayer == null)
        {
            Log.PrintError("myPlayer is null");
            return;
        }
        Player.myPlayer.inventory.AddItemToInventory(this);
        OnPickup(Player.myPlayer);
    }

    /// <summary>
    /// Called by the client after it owns this.
    /// </summary>
    /// <param name="m"></param>
    public virtual void OnPickup(Mob m)
    {
        if(!GetVar<bool>("onGround"))
            return;
        Log.Print("Attempted to pickup item");
        SetVar("onGround", false);
        GetComponent<MeshRenderer>().enabled = false;
    }

}
