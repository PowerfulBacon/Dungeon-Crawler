using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunCallbacks : MonoBehaviourPunCallbacks
{

    //================================================
    //! Connection to master
    //================================================

    public delegate void OnConnectedToMasterDelegate();
    public static OnConnectedToMasterDelegate onConnectedToMaster;

    public override void OnConnectedToMaster()
    {
        onConnectedToMaster();
    }

    //================================================
    //! Failure to join random room
    //================================================

    public delegate void onJoinRandomFailedDelegate(short returnCode, string message);
    public static onJoinRandomFailedDelegate onJoinRandomFailed;

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        onJoinRandomFailed(returnCode, message);
    }

    //================================================
    //! Connected to a room
    //================================================

    public delegate void OnPlayerEnteredRoomDelegate(Player newPlayer);
    public static OnPlayerEnteredRoomDelegate onPlayerEnteredRoom;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        onPlayerEnteredRoom(newPlayer);
    }

    //================================================
    //! Room created
    //================================================

    public delegate void OnCreatedRoomDelegate();
    public static OnCreatedRoomDelegate onCreatedRoom;

    public override void OnCreatedRoom()
    {
        onCreatedRoom();
    }

}
