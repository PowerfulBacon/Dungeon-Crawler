using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// god, our current network architecture is stupid considering security is not a high issue.
/// Giving the client that controls the mob complete ownership of it would be much better since then
/// they can update movement in real time and not have to wait for the server to process it.
/// Would also reduce load on the server.
/// However, would make networking more difficult since everything would have to handle networking.
/// 
/// Wait: We need to tell clients what they pick up anyway, so uhhh like we might as well just give clients the ability to control
/// themselves!!
/// </summary>
public class Client : MonoBehaviourPunCallbacks
{

    public Preferences prefs = new Preferences();   //Client preferences

    public Player punPlayer;

    public Mob mob; //The current mob we are controlling (ONLY AVAILABLE ON SERVER DONT USE IN RPCs)

    public KeyMap keyMap = new KeyMap();   //What keys are being pressed by our attatched client

    public void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        ServerReleaseClient();
    }

    /// <summary>
    /// Destroy clients to remove them from the game.
    /// </summary>
    void OnDestroy()
    {

        LogHandler.Log("Client removed from game");

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CloseConnection(punPlayer);
        }
    }

    [PunRPC]
    public void ServerUpdateKeyUpdateRPC(string buttonString, bool pressed)
    {
        keyMap.SetKeyState(buttonString, pressed);
    }

    [PunRPC]
    public void ServerUpdateAxisUpdateRPC(string buttonString, float value)
    {
        keyMap.SetAxisState(buttonString, value);
    }

    [PunRPC]
    public void OnTransferRPC()
    {
        if (photonView.IsMine)
        {
            NetworkController.localClient = this;
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
            LogHandler.Log("Local client located.");
        }
    }

    [PunRPC]
    public void TransferToMobRPC(int atom_id)
    {
        if (!photonView.IsMine)
        {
            LogHandler.Log("This is not mine.");
            return;
        }

        //Find our new mob
        Transform foundTransform = null;
        foreach (Mob mob in FindObjectsOfType<Mob>())
        {
            if (mob.atom_id == atom_id)
            {
                foundTransform = mob.cameraAttachmentPoint ? mob.cameraAttachmentPoint.transform : mob.transform;
                break;
            }
        }
        if (foundTransform == null)
        {
            LogHandler.Log("COULD NOT LOCATE MOB WITH ID");
        }
        else
        {
            transform.SetParent(foundTransform);
        }
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

    }

    public void ServerReleaseClient()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            LogHandler.Log("WARNING! Non server attempted to relase client!!!!");
            return;
        }
        if (mob != null)
        {
            mob.OnClientReleased();
        }
        //Create mob and transfer ownership to the recieving client
        Observer observer = PhotonNetwork.Instantiate("Observer", Vector3.zero, Quaternion.identity).GetComponent<Observer>();
        observer.photonView.TransferOwnership(punPlayer);
        photonView.RPC("TransferToMobRPC", punPlayer, observer.atom_id);
        mob = observer;
    }

}
