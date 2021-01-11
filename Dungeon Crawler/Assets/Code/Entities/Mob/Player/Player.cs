using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob
{

    public static Player myPlayer;

    public Dictionary<string, object> flags = new Dictionary<string, object>();
    public List<PlayerModule> modules = new List<PlayerModule> {
        new PlayerMovement(),
        new PlayerCamera(),
        new PlayerInteraction(),
        new PlayerUserInterfaceModule(),
    };

    public Stats stats = new Stats();
    public Skills skills = new Skills();

    public InventorySystem inventory;

    //The item we are holding, pretty useful to know
    public Item heldItem;

    // Start is called before the first frame update
    // NOTE: Overrides the default start function which applies models lol
    void Start()
    {
        //Create the inventory system
        inventory = new InventorySystem(this);
        //Initialise the modules
        foreach (PlayerModule module in modules)
        {
            module.OnInitialise(this);
        }
    }

    // Update is called once per frame
    // * > CLIENT UPDATES < *
    void Update()
    {

        if (!photonView.IsMine)
            return;

        foreach (PlayerModule module in modules)
        {
            module.OnUpdate(this);
        }

    }

    public override void GainOwnership()
    {
        if(myPlayer != null)
        {
            if(photonView.IsMine)
            {
                if(PhotonNetwork.IsMasterClient)
                {
                    //We were transfered someone elses player (assuming they disconnected.) Delete it.
                    Log.PrintDebug("Destroying player object (assuming transfered).");
                    PhotonNetwork.Destroy(photonView);
                }
                else
                {
                    Log.PrintError("Major error, we were transfered ownership.");
                }
                return;
            }
        }
        //I see myself!
        myPlayer = this;
    }

    /// <summary>
    /// Sets the item that we are holding.
    /// heldItem is only known locally, however we need to tell the other clients what model our hand should have.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="itemDropped">True if the item was dropped. Prevents onDequip calls (needs refactoring)</param>
    public void SetHeldItem(Item item, bool itemDropped)
    {
        if(!photonView.IsMine)
        {
            Log.PrintError("We tried to set someone elses held item for some reason.");
            return;
        }
        if(item != null && !item.photonView.IsMine)
        {
            Log.PrintError("We do not have control of the object attempted to put in my hand.");
            return;
        }
        if(heldItem == item)
            return;
        if(heldItem != null && !itemDropped)
            heldItem.OnDequip(this);
        //Set the variable
        heldItem = item;
        //Equip held item
        if(heldItem != null)
            heldItem.OnEquip(this);
    }


    /// <summary>
    /// Get the use delay (Take into account item use delay TODO)
    /// </summary>new 
    /// <returns></returns>
    public float GetUseDelay()
    {
        return 0.3f;
    }

}
