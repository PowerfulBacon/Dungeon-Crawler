using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob
{

    public Dictionary<string, object> flags = new Dictionary<string, object>();
    public List<PlayerModule> modules = new List<PlayerModule> { new PlayerMovement() };

    public Stats stats = new Stats();
    public Skills skills = new Skills();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!photonView.IsMine)
            return;

        foreach (PlayerModule module in modules)
            module.OnUpdate(this);

    }

    public new void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }

}
