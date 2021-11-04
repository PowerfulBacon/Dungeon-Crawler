using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectLoader : MonoBehaviourPunCallbacks
{

    //This is a super botched way to get around multi thread limitations but it just works ;)
    //Key = Functions, Value = parameters
    public static Dictionary<Client, string> clientsToSpawn = new Dictionary<Client, string>();

    // Update is called once per frame
    void Update()
    {

        if (!PhotonNetwork.IsMasterClient)
            return;

        for (int i = clientsToSpawn.Count - 1; i >= 0; i--)
        {
            Client client = clientsToSpawn.Keys.ElementAt(i);
            LogHandler.Log("Spawning human");
            Mob human = PhotonNetwork.Instantiate(clientsToSpawn[client], Vector3.zero, Quaternion.identity).GetComponent<Mob>();
            human.TransferClientToMob(client);
            clientsToSpawn.Remove(client);
        }
        
    }

    public static void QueueSpawnAndAttatch(string type, Client client)
    {
        if (clientsToSpawn.ContainsKey(client))
        {
            clientsToSpawn[client] = type;
        }
        else
        {
            clientsToSpawn.Add(client, type);
        }
    }

}
