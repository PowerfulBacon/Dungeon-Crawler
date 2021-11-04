using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

/// <summary>
/// An existing entity in the world
/// The base for many other classes
/// </summary>
public class Entity : MonoBehaviourPunCallbacks
{

    //FOR INFREQUENTLY UPDATED VARIABLES ONLY
    public Dictionary<string, object> variables = new Dictionary<string, object>();

    /// <summary>
    /// The name of the model in the resources file.
    /// </summary>
    public string model;

    // ==========================
    // Start:
    // - Client method
    // Initiates the models of objects. (Do not change networked variables in start please.)
    // ==========================
    
    private void Start()
    {
        SetModel();
        ApplyModel();
        ClientInit();
    }

    //Generic unity update handler hook
    private void Update()
    {
        LocalUpdate();
        //Update for the owner
        if(photonView.IsMine)
        {
            OwnerUpdate();
        }
    }

    /// <summary>
    /// Performs a local update.
    /// This should be for rendering updates, happens if we own the object or not.
    /// </summary>
    protected virtual void LocalUpdate()
    { }

    /// <summary>
    /// Called if we own this object.
    /// </summary>
    protected virtual void OwnerUpdate()
    { }

    protected virtual void ClientInit()
    { }

    protected virtual void SetModel()
    {
        model = "sword";
    }

    protected virtual void ApplyModel()
    {
        //TODO: Asset Caching.
        Mesh mesh = Resources.Load<Mesh>($"Models/{model}");
        GetComponent<MeshFilter>().mesh = mesh;
        //Hitbox
        gameObject.AddComponent<BoxCollider>();
    }

    // ==========================
    // Creator helpers.
    // ==========================

    public static void CreateEntity(Type type)
    {
        CreateEntity(type, Vector3.zero, Quaternion.identity);
    }

    public static void CreateEntity(Type type, Vector3 position, Quaternion rotation)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Log.PrintError($"Attempted to create a {type.ToString()}. Permission denied.");
            return;
        }
        PhotonNetwork.Instantiate("NetworkPrefab/blank", position, rotation, 0, new object[]{type.ToString()});
    }

    // ==========================
    // Normal stuff from this point on
    // ==========================

    public virtual void OnInitialise()
    {
        //Attach outself to the photonview
        photonView.ObservedComponents.Add(this);
    }

    /// <summary>
    /// Destroys the entity if we have access to it.
    /// </summary>
    public virtual void Destroy()
    {
        if(!photonView.IsMine)
        {
            Log.PrintError($"Attempted to destroy {ToString()} but do not have ownership.");
            return;
        }
        //Destroy.
        PhotonNetwork.Destroy(photonView);
    }

    /// <summary>
    /// Called every update by the server.
    /// Note that its called by the server even if the server does not have control over the object.
    /// Should not be used for things like inputs, but checking if the player is dead etc.
    /// 
    /// REQUIRES PROCESSING TO BE STARTED OR THIS WILL NOT RUN!
    /// </summary>
    public virtual void OnUpdate(float deltaTime)
    {

    }

    //VARIABLE TRACKERS
    //THIS IS FOR NETWORKED VARIABLES.

    //Updates everything on the client
    //Probably quite the lag machine. (uses a lot of bandwidth D: )
    public void SendAllVars(Photon.Realtime.Player client)
    {
        foreach (string variable in variables.Keys)
        {
            photonView.RPC("RPCAddVar", client, variable, variables[variable]);
        }
    }

    //For clients to recieve
    [PunRPC]
    public void RPCAddVar(string name, object data)
    {
        variables.Add(name, data);
        Log.PrintDebug($"Added {name} as requested by server.");
        OnVarAdded(name, data);
    }

    /// <summary>
    /// Override for special functionality when vars are added.
    /// Executed both server and client side.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="data"></param>
    protected virtual void OnVarAdded(string name, object data)
    { }

    //For the owner to say 'yayyy lets track this'
    public void AddVar(string name, object data)
    {
        if(!photonView.IsMine)
        {
            Log.PrintError($"Attempting to add variable {name} to object {ToString()} without control of it. Request denied.");
            return;
        }
        //Debug log for no reason
        Log.PrintDebug($"(MASTER CLIENT) Creating variable {name} on {ToString()}");
        //Add the variable
        variables.Add(name, data);
        //Tell the clients that we want to track this variable.
        photonView.RPC("RPCAddVar", RpcTarget.Others, name, data);
        //Call the var added thing
        OnVarAdded(name, data);
    }

    public object GetVar(string name)
    {
        return variables[name];
    }

    public void SetVar(string name, object newVal)
    {
        if(!photonView.IsMine)
        {
            Log.PrintError($"Error: Attempted to modify object we dont own. {ToString()}, {name}: {newVal}");
            return;
        }
        variables[name] = newVal;
        photonView.RPC("RPCSetVar", RpcTarget.Others, name, newVal);
    }

    [PunRPC]
    public void RPCSetVar(string name, object newVal)
    {
        variables[name] = newVal;
    }

    public T GetVar<T>(string name)
    {
        try
        {
            return (T)variables[name];
        }
        catch (Exception e)
        {
            Log.PrintError($"Failed to process variable named {name}.\n{e.Message}\n{e.StackTrace}");
            return default(T);
        }
    }

    public void RemoveVar(string name)
    {
        try
        {
            variables.Remove(name);
        }
        catch
        {
            Log.Print("Could not delete variable, as it doesn't exist");
        }
    }

    public virtual void GainOwnership()
    { }

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

}
