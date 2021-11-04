using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All atoms need 
/// 1 - PhotonView that is monitoring their transform
/// ok thats it
/// </summary>
public class Atom : MonoBehaviourPunCallbacks, IPunObservable
{

    // TODO: Why are we sending this 20 times per second???????
    public static int unique_atom_id;
    public int atom_id;

    // Start is called before the first frame update
    void Start()
    {

        if (photonView == null)
        {
            gameObject.AddComponent<PhotonView>();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            atom_id = unique_atom_id++;
        }

        Initialize();

    }

    public virtual void Initialize() { }

    /// <summary>
    /// Write our ID to the clients literally just so they can be located :(
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            int id_to_send = unique_atom_id;
            stream.Serialize(ref id_to_send);
        }
        else
        {
            int id_to_receive = 0;
            stream.Serialize(ref id_to_receive);
            unique_atom_id = id_to_receive;
        }
    }

}
