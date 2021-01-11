using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkController : Subsystem
{

    public static Client localClient;
    public static List<Client> clients = new List<Client>();

    public NetworkController()
    {
        name = "Network Controller";
    }

    public override void Initialize()
    {
        base.Initialize();
        SetupDelegates();
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// Setup delegate actions
    /// </summary>
    private void SetupDelegates()
    {
        PunCallbacks.onConnectedToMaster += OnConnectedToMaster;
        PunCallbacks.onJoinRandomFailed += OnJoinRandomFailed;
        PunCallbacks.onPlayerEnteredRoom += OnClientConnected;
        PunCallbacks.onCreatedRoom += RoomCreated;
    }

    /// <summary>
    /// Connected to the master server
    /// </summary>
    public void OnConnectedToMaster()
    {
        LogHandler.Log("Connected to master server");
        JoinOrCreate("default");
        LogHandler.Log($"Connected to region {PhotonNetwork.CloudRegion}");
        LogHandler.Log($"Ping: {PhotonNetwork.GetPing()} ms");
        LogHandler.Log($"Send Rate: {PhotonNetwork.SendRate} packets per second");
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        LogHandler.Log("Failed to join random room");
        CreateRoom();
    }

    /// <summary>
    /// Connect to a random room
    /// </summary>
    public void ConnectToRandom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    /// <summary>
    /// Creates a room
    /// </summary>
    public void CreateRoom()
    {
        LogHandler.Log("Creating room");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        PhotonNetwork.CreateRoom("default", roomOptions, TypedLobby.Default);
    }

    /// <summary>
    /// Join or creates a room by name
    /// </summary>
    /// <param name="name"></param>
    public void JoinOrCreate(string name)
    {
        LogHandler.Log($"Joining (or creating if unavailable) room {name}");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(name, roomOptions, TypedLobby.Default);
    }

    /// <summary>
    /// Called when a room has been created
    /// </summary>
    public void RoomCreated()
    {
        LogHandler.Log("Room created!");
        //Attatch client for ourselves
        GameObject clientObject = Resources.Load<GameObject>("Client"); //Todo: Load this once
        Client client = PhotonNetwork.Instantiate("Client", Vector3.zero, Quaternion.identity).GetComponent<Client>();
        client.punPlayer = PhotonNetwork.LocalPlayer;
        client.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        client.OnTransferRPC();
        clients.Add(client);
    }

    /// <summary>
    /// Player connected to the room
    /// </summary>
    /// <param name="newPlayer"></param>
    public void OnClientConnected(Player newPlayer)
    {
        LogHandler.Log("User connecteds to room");
        if (!PhotonNetwork.IsMasterClient)
            return;
        //Create client for player
        GameObject clientObject = Resources.Load<GameObject>("Client"); //Todo: Load this once
        Client client = PhotonNetwork.Instantiate("Client", Vector3.zero, Quaternion.identity).GetComponent<Client>();
        client.punPlayer = newPlayer;
        client.photonView.TransferOwnership(newPlayer);
        client.photonView.RPC("OnTransferRPC", newPlayer);
        clients.Add(client);
    }

}
