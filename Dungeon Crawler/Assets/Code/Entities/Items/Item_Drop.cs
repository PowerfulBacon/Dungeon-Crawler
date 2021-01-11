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
    
    /// <summary>
    /// Safe dropping method that handles dropping up items.
    /// Different effects depending on client or server.
    /// Called on a client when they want to pick something up.
    /// </summary>
    public void DropItem()
    {

        if(!photonView.IsMine)
        {
            Log.PrintError("Error: We cannot drop an item we don't have ownership of.");
            return;
        }

        Log.PrintDebug("Dropping");

        //Tell everyone we have been dropped
        photonView.RPC("RPCDroppedAll", RpcTarget.All);

        //Release our ownership
        photonView.TransferOwnership(PhotonNetwork.MasterClient);

        //Remove from inventory
        RemoveFromInventory();

    }

    [PunRPC]
    public void RPCDroppedAll()
    {
        Log.PrintDebug("Item was dropped, rendering enabled.");
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        //Set parent
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.SetParent(null);
    }

    private void RemoveFromInventory()
    {
        Player.myPlayer.inventory.RemoveItemFromInventory(this);
    }

}
