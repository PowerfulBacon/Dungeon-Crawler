using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Settings used to connect to the server
/// </summary>
public class ServerConnectSettings
{

    public ServerConnectSettings(string name = "default", bool create = false)
    {
        roomName = name;
        createRoomIfNoneAvailable = create;
    }

    public string roomName = "default";
    public bool createRoomIfNoneAvailable = false;

}

/// <summary>
/// The thing that does the connecting
/// </summary>
public class ServerConnector
{

    public const string GAMEVERSION = "0.01";


    public void ConnectToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = GAMEVERSION;
    }


    public void ConnectToServer(ServerConnectSettings connectSettings)
    {
        if (connectSettings.createRoomIfNoneAvailable)
        {
            RoomOptions roomOptions = new RoomOptions();
            PhotonNetwork.JoinOrCreateRoom(connectSettings.roomName, roomOptions, new TypedLobby(connectSettings.roomName, LobbyType.Default));
        }
    }

}
