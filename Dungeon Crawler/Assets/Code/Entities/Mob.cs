using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviourPunCallbacks, IPunObservable
{

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        return;
    }

}
