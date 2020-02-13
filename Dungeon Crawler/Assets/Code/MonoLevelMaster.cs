using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonoLevelMaster : MonoBehaviourPunCallbacks, IPunObservable
{

    public int levelSeed;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(levelSeed);
        }
        else
        {
            levelSeed = (int)stream.ReceiveNext();
        }
    }
}
