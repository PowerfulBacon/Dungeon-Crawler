using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SubsystemMasterMono : MonoBehaviourPunCallbacks
{

    // Start is called before the first frame update
    void Start()
    {
        foreach (Subsystem subsystem in Master.subsystems.Values)
        {
            StartCoroutine("StartSubroutine", subsystem);
        }
    }

    public IEnumerator StartSubroutine(Subsystem system)
    {
        yield return system.UpdateThreadMaster();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Master.subsystems["networkManagement"].Request("OnCreateRoomFailed", new object[] { returnCode, message });
    }

    public override void OnJoinedRoom()
    {
        Master.subsystems["networkManagement"].Request("OnJoinedRoom");
    }

    public override void OnCreatedRoom()
    {
        Master.subsystems["networkManagement"].Request("OnCreatedRoom");
    }

    public override void OnConnectedToMaster()
    {
        Master.subsystems["networkManagement"].Request("OnConnectedToMaster");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Master.subsystems["networkManagement"].Request("OnJoinRoomFailed", new object[] { returnCode, message });
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Master.subsystems["networkManagement"].Request("OnPlayerEnteredRoom", newPlayer);
    }

}
