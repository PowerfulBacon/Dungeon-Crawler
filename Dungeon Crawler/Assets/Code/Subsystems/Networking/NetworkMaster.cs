using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkMaster : Subsystem
{

    public static Dictionary<string, ConnectedUser> playerKeys = new Dictionary<string, ConnectedUser>();

    public ServerConnector serverConnector = new ServerConnector();

    public NetworkMaster(string name = "") : base(name)
    {
    }

    public override void Initialise()
    {
        Request("MasterConnect", new ServerConnectSettings("Test1", true));
    }

    protected override void Update()
    {

        //Handle subsystem queries
        if (subsystemQuery.Keys.Count <= 0)
            return;

        //Get the query
        string queryName = subsystemQuery.Keys.ElementAt(0);
        object queryData = subsystemQuery[queryName];

        //Execute
        switch (queryName)
        {
            case "MasterConnect":
                serverConnector.ConnectToMaster();
                break;

            case "OnConnectedToMaster":
                Log.ServerMessage("Connected to master server!");
                //Debug join random room
                Request("ServerConnect", new ServerConnectSettings("default", true));
                break;

            case "ServerConnect":
                Log.ServerMessage("Attempting to create server");
                serverConnector.ConnectToServer((ServerConnectSettings)queryData);
                break;

            case "OnCreatedRoom":
                //Execute the onPlayerEnteredCode and anything else
                if (!PhotonNetwork.IsMasterClient)
                    break;
                Log.ServerMessage("Instantiating Player");
                //Create outselves a player
                //Normally we shouldn't instantiate like this and use our helpers, but we need to transfer shit. (Actually we should use the other method, this is kinda shitcodey)
                var cooObject = PhotonNetwork.Instantiate("NetworkPrefab/Player", new Vector3(0, 2.75f, 0), Quaternion.identity, 0);
                Player.myPlayer = cooObject.GetComponent<Player>();
                Player.myPlayer.OnInitialise();
                //Generate a random seed for the level
                LevelGenerator.currentSeed = Random.Range(0, 1000000000);
                Master.subsystemMaster.RPCGenerateLevel(LevelGenerator.currentSeed, 64);
                //Debug create blob
                Entity.CreateEntity(typeof(Blob), new Vector3(0, 2.0f, 0), Quaternion.identity);
                Entity.CreateEntity(typeof(Blob), new Vector3(1.0f, 2.0f, 0), Quaternion.identity);
                break;

            case "OnPlayerEnteredRoom":
                //Create a player just for them <3
                Log.ServerMessage("Instantiating Player"); 
                var instantiatedObject = PhotonNetwork.Instantiate("NetworkPrefab/Player", new Vector3(0, 2.75f, 0), Quaternion.identity, 0);
                instantiatedObject.GetComponent<Player>().OnInitialise();
                instantiatedObject.GetPhotonView().TransferOwnership((Photon.Realtime.Player)queryData);
                //Ask them to generate the level
                Master.subsystemMaster.photonView.RPC("RPCGenerateLevel", (Photon.Realtime.Player)queryData, LevelGenerator.currentSeed, 64);
                //Full update all objects
                foreach(Entity entity in Object.FindObjectsOfType<Entity>())
                {
                    entity.SendAllVars((queryData as Photon.Realtime.Player));
                }
                break;
            
            case "OnJoinedRoom":
                Log.ServerMessage("Connected to room.");
                break;

            default:
                Log.PrintError($"Unrecognised query [{queryName}] in networkGenerator, deleting");
                break;
        }

        subsystemQuery.Remove(queryName);

    }

}
