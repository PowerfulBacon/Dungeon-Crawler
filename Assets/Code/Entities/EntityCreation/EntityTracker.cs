using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

/// <summary>
/// What is the point of this script?
/// Since we have a lot of items and I am too lazy to create a seperate network prefab for all of them,
/// this script simply holds what type of object we need to spawn and which variables to add.
/// As soon as its created it just gets destroyed.
/// Recieves custom data from the server about variables and adds them as needed.
/// </summary>
public class EntityTracker : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {

        photonView.InstantiationData = info.photonView.InstantiationData;

        //Collect data
        object[] instantiationData = info.photonView.InstantiationData;
        Type objectType = Type.GetType((string)instantiationData[0]);
        Log.PrintDebug($"Making the thing, TYPE: {(string)instantiationData[0]}, typeof: {objectType.Name}");
        Entity createdEntity = gameObject.AddComponent(objectType) as Entity;

        //Check just in case
        if(createdEntity == null)
        {
            Log.PrintError("Something has gone horribly wrong, instantiation of object failed.");
            return;
        }

        //Call initialise if required
        if(photonView.IsMine)
        {
            createdEntity.OnInitialise();
        }

        //Purpose fufilled.
        Destroy(this);
    }

}
