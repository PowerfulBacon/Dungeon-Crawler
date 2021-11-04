using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunCallbacks : MonoBehaviourPunCallbacks
{

    public delegate void OnConnectedToMasterDelegate();
    public static OnConnectedToMasterDelegate onConnectedToMaster;

    public override void OnConnectedToMaster()
    {
        onConnectedToMaster();
    }

}
