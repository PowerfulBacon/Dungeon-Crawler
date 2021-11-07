using Dungeon_Crawler.Assets.Code.UserInterface.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mob : Entity
{

    //Server only variable
    //Tracks the number of mobs
    //Allows for each mob to be assigned a unique ID
    private static int playerNumbers = 0;
    //Server only!
    //A static dictionary that allows for looking up mobs by their unique ID
    //TODO: On destroy remove from this list.
    private static Dictionary<int, Mob> mobLookup = new Dictionary<int, Mob>();

    //The client in control of this mob.
    //TODO Client
    public ConnectedUser client;

    //Server variable
    //Factions this mob is a part of
    //Default get no factions
    public virtual List<string> factions { get; } = new List<string>();

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

    /// <summary>
    /// Owner update that handles automated mob actions.
    /// </summary>
    protected override void OwnerUpdate()
    {
        //Update AI
        if(client == null)
        {
            HandleMobAction();
        }
    }

}
