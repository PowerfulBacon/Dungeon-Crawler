using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyMap
{

    public Dictionary<string, float> axisValues = new Dictionary<string, float>
    {
        { "Horizontal", 0 },
        { "Vertical", 0 },
        { "MouseX", 0 },
        { "MouseY", 0 }
    };
    public Dictionary<string, bool> directValues = new Dictionary<string, bool> 
    {
        { "LeftMouse", false },
        { "RightMouse", false },
        { "Space", false },
        { "Tab", false }
    };

    private bool HasAxisChanged(string keyName, float newValue)
    {
        return axisValues[keyName] != newValue;
    }

    private bool HasDirectChanged(string keyName, bool newValue)
    {
        return directValues[keyName] != newValue;
    }

    public float GetAxisState(string keyName)
    {
        return axisValues[keyName];
    }

    public bool GetKeyState(string keyName)
    {
        return directValues[keyName];
    }

    public void SetAxisState(string keyName, float value)
    {
        if (!axisValues.ContainsKey(keyName))
        {
            LogHandler.Log($"!Keystate '{keyName}' not found!");
            return;
        }

        if (!HasAxisChanged(keyName, value))
        {
            return;
        }

        axisValues[keyName] = value;

        //If we aren't the master client lets transmit ourselves to it
        if (!PhotonNetwork.IsMasterClient)
        {
            NetworkController.localClient.photonView.RPC("ServerUpdateAxisUpdateRPC", RpcTarget.MasterClient, keyName, value);
        }

    }

    public void SetKeyState(string keyName, bool value)
    {
        if (!directValues.ContainsKey(keyName))
        {
            LogHandler.Log($"!Keystate '{keyName}' not found!");
            return;
        }

        if (!HasDirectChanged(keyName, value))
        {
            return;
        }

        directValues[keyName] = value;

        //If we aren't the master client lets transmit ourselves to it
        if (!PhotonNetwork.IsMasterClient)
        {
            NetworkController.localClient.photonView.RPC("ServerUpdateKeyUpdateRPC", RpcTarget.MasterClient, keyName, value);
        }

    }

}
