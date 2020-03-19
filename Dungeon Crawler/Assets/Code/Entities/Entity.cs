using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

/// <summary>
/// An existing entity in the world
/// The base for many other classes
/// </summary>
public class Entity : MonoBehaviourPunCallbacks, IPunObservable
{

    Dictionary<string, bool> requiresUpdate;
    Dictionary<string, object> variables;

    public virtual void OnInitialise()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public void AddVar(string name, object data)
    {
        requiresUpdate.Add(name, true);
        variables.Add(name, data);
    }

    public object GetVar(string name)
    {
        return variables[name];
    }

    public void SetVar(string name, object newVal)
    {
        variables[name] = newVal;
        requiresUpdate[name] = true;
    }

    public T GetVar<T>(string name)
    {
        try
        {
            return (T)variables[name];
        }
        catch
        {
            Log.Print("Failed to convert variable to required type");
            return default(T);
        }
    }

    public void RemoveVar(string name)
    {
        try
        {
            requiresUpdate.Remove(name);
            variables.Remove(name);
        }
        catch
        {
            Log.Print("Could not delete variable, as it doesn't exist");
        }
    }

    public virtual void ReleaseOwnership()
    {
        if (PhotonNetwork.MasterClient == null)
        {
            //Maybe we should try rejoin and give option to host server from current state or something
            Log.Print("Hey, no server master found. Potentially disconnected from server.", false);
            return;
        }
        //Give to server
        photonView.TransferOwnership(PhotonNetwork.MasterClient);
        Log.Print("Released ownership of object " + gameObject.name);
    }

    public virtual void RequestOwnership()
    {
        photonView.RequestOwnership();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            for (int i = 0; i < requiresUpdate.Count; i++)
            {
                if (!requiresUpdate.Values.ElementAt(i))
                    continue;
                //Send the name, then the value
                stream.SendNext(variables.Keys.ElementAt(i));
                stream.SendNext(variables.Values.ElementAt(i));
            }
        }
        else
        {
            while (stream.PeekNext() != null)
            {
                string varName = (string)stream.ReceiveNext();
                object varValue = stream.ReceiveNext();
                if (!variables.ContainsKey(varName))
                {
                    variables.Add(varName, varValue);
                    Log.Print("Added new variable " + varName + " to object " + gameObject.name);
                }
                else
                    variables[varName] = varValue;
            }
        }

    }

}
