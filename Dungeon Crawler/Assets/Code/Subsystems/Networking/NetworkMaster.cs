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

    public override void Initialise()
    {
        Request("MasterConnect", new ServerConnectSettings("Test1", true));
    }

    protected override void Update()
    {

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
                Debug.Log("Connected to master server!");
                //Debug join random room
                Request("ServerConnect", new ServerConnectSettings("default", true));
                break;

            case "ServerConnect":
                Debug.Log("Attempting to create server");
                serverConnector.ConnectToServer((ServerConnectSettings)queryData);
                break;

            case "OnCreatedRoom":
                //Execute the onPlayerEnteredCode and anything else
                if (!PhotonNetwork.IsMasterClient)
                    return;
                LevelGenerator.currentSeed = Random.Range(0, 1000000000);
                goto playerEnteredRoom;

            case "OnPlayerEnteredRoom":
                playerEnteredRoom:
                //Create a new player holder for the person
                if (!PhotonNetwork.IsMasterClient)
                    break;
                //Ask them to generate the level
                Master.subsystemMaster.photonView.RPC("RPCGenerateLevel", RpcTarget.All, LevelGenerator.currentSeed, 256);
                break;

            default:
                Debug.LogError("Unrecognised query [" + queryName + "] in networkGenerator, deleting");
                break;
        }

        subsystemQuery.Remove(queryName);

    }

}
