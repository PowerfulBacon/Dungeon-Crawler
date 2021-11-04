using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SubsystemMasterMono : MonoBehaviourPunCallbacks
{

    public PhotonView photonView = null;

    // Start is called before the first frame update
    void Start()
    {

        photonView = gameObject.AddComponent<PhotonView>();

        foreach (Subsystem subsystem in Master.subsystems.Values)
        {
            StartCoroutine("StartSubroutine", subsystem);
        }
    }

    public IEnumerator StartSubroutine(Subsystem system)
    {
        yield return system.UpdateThreadMaster();
    }

    [PunRPC]
    public void RPCGenerateLevel(int seed, int levelSize = 255)
    {
        Log.Print("Recieved");
        Master.subsystems["levelGeneration"].Request("generateLevel", new object[] { seed, levelSize });
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
