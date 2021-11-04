using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : Subsystem
{

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
    }

    /// <summary>
    /// Connected to the master server
    /// </summary>
    public void OnConnectedToMaster()
    {
        LogHandler.Log("Connected to master server");
    }

}
