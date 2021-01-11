using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// = = = = NETWORKING REWORK PLAN = = = =
/// 
/// 1) On gaining ownership of a mob server transfers the mob to client
/// 2) Mobs with no client attached need to be owned by the server
/// 3) When a client disconnects their mob needs to be somehow claimed by the server
///  - We shouldn't need to get clients from other mobs unless the server is transfering them in some way
///  - The server needs some way to detach clients
/// 
/// </summary>
public class Mob : Atom
{

    

    public Client client;   //The client attatched to this mob. Needs to be known by the server and the client in control of this mob

    public GameObject cameraAttachmentPoint;    //Where the Client gets attached to

    public override void Initialize()
    {
        base.Initialize();
        if (cameraAttachmentPoint == null)
        {
            cameraAttachmentPoint = gameObject;
        }

        //Mobs need to be using request ownership
        photonView.OwnershipTransfer = OwnershipOption.Request;
    }

    private void OnDestroy()
    {

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (client != null)
        {
            client.ServerReleaseClient();
        }
    }


    /// <summary>
    /// LOCAL
    /// Called every frame
    /// </summary>
    private void Update()
    {
        if (photonView.IsMine)
        {
            Life();
        }
        LocalUpdate();
    }

    /// <summary>
    /// LOCAL
    /// Update that is called locally, SHOULD NOT TOUCH NETWORKING
    /// !!! This is for animations, rendering and NOT GAME LOGIC !!!
    /// </summary>
    public virtual void LocalUpdate()
    {
        
    }

    /// <summary>
    /// [REFACTORED - CLIENT THAT CONTROLS US]
    /// Life of the mob (essentially update).
    /// This is server updated
    /// </summary>
    public virtual void Life()
    {
        if (client == null)
            NpcLife();
        else
            ClientLife();
    }

    /// <summary>
    /// SERVER (No client is controlling so server is controller)
    /// Code to run if no client is controlling the mob
    /// </summary>
    public virtual void NpcLife()
    {

    }

    /// <summary>
    /// CONTROLLING CLIENT
    /// Code that runs if a client is in control of the mob.
    /// </summary>
    public virtual void ClientLife()
    {

    }

    /// <summary>
    /// CONTROLLING CLIENT
    /// The client currently controlling this mob is no longer controlling this mob.
    /// </summary>
    public virtual void OnClientReleased()
    {
        client = null;
        photonView.TransferOwnership(PhotonNetwork.MasterClient);
    }

    /// <summary>
    /// SERVER
    /// Puts a client in control of this mob.
    /// </summary>
    /// <param name="client">
    /// The client to put in control
    /// </param>
    /// <remarks>
    /// Override_existing doesn't work after network refit lol
    /// </remarks>
    public void TransferClientToMob(Client newClient, bool override_existing = false)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            LogHandler.Log("Non server mob attempting to transfer ownership.");
            return;
        }

        if (client != null)
        {
            if (!override_existing)
            {
                LogHandler.Log($"Error: Cannot transfer client {newClient.punPlayer.UserId} into mob {name}, it is already controlled by client {client.punPlayer.UserId}");
                return;
            }
            else
            {
                client.ServerReleaseClient();
            }
        }

        client = newClient;
        client.photonView.RPC("TransferToMobRPC", newClient.punPlayer, atom_id);

    }

}