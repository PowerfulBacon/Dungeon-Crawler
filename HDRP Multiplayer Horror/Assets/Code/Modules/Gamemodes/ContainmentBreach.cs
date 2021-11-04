using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainmentBreach : Gamemode
{

    private const string PLAYER_CONTROLLED_SCP_COUNT = "Player Controlled SCPs";
    private const string SCP_173_ENABLED = "SCP-173 Enabled";

    public ContainmentBreach()
    {
        name = "Containment Breach";
        settings = new Dictionary<string, object> 
        { 
            { PLAYER_CONTROLLED_SCP_COUNT, 0 },
            { SCP_173_ENABLED, false }
        };

    }

    public override void OnStart()
    {
        base.OnStart();

        //Create Players
        foreach (Client client in NetworkController.clients)
        {
            ObjectLoader.QueueSpawnAndAttatch("human", client);
        }

    }

}
