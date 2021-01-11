using Dungeon_Crawler.Assets.Code.UserInterface.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mob : Entity
{

    private static int playerNumbers = 0;
    //Server only!
    private static Dictionary<int, Mob> mobLookup = new Dictionary<int, Mob>();

    protected bool dead = false;

    /// <summary>
    /// Serverside init.
    /// </summary>
    public override void OnInitialise()
    {
        base.OnInitialise();
        AddVar("health", 100);
        playerNumbers ++;
        AddVar("mobId", playerNumbers);
    }

    public void ToChat(string message)
    {
        photonView.RPC("RPCChatClient", RpcTarget.All, message);
    }

    [PunRPC]
    public void RPCChatClient(string message)
    {
        if(photonView.IsMine)
        {
            Chat.ToChat(message);
        }
    }

    protected override void OnVarAdded(string name, object data)
    {
        //Mob ID custom things
        if(name == "mobId")
        {
            if(mobLookup.ContainsKey((int) data)) return;
            mobLookup.Add((int)data, this);
        }
    }

    /// <summary>
    /// Gets a mob by their ID.
    /// </summary>
    /// <param name="mobId"></param>
    /// <returns></returns>
    public static Mob GetMobById(int mobId)
    {
        return mobLookup[mobId];
    }

}
