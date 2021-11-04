using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnershipCallbackTracker : MonoBehaviour, IPunOwnershipCallbacks
{

    private void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnOwnershipRequest(PhotonView targetView, Photon.Realtime.Player requestingPlayer)
    {
        return;
    }

    public void OnOwnershipTransfered(PhotonView targetView, Photon.Realtime.Player previousOwner)
    {
        Entity entity = targetView.GetComponent<Entity>();
        //The thing is an entity
        if(entity != null)
        {
            if(entity.photonView.IsMine)
            {
                Log.PrintDebug($"We just gained ownership of {entity.ToString()}");
                entity.GainOwnership();
            }
            else
            {
                Log.PrintDebug($"Someone else just gained ownership of {entity.ToString()}");
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
