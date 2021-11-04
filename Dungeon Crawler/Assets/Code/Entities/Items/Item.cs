using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Anything that can be put in the inventory ---MUST--- inherit this :) <3
/// </summary>
public partial class Item : Entity
{

    public const float WORLD_SCALE = 1.0f;
    public const float EQUIPPED_SCALE = 0.5f;

    public static Material globalMaterial { get; set; }

    public string itemName { get; set; } = "generic";

    public string description = "No description set.";

    private static float itemFallSpeed = 2.0f;

    private PhotonTransformView viewTransform;

    /// The slot of the players inventory this item is inside.
    public int inventorySlot = -1;

    /// The name of the icon we are using (located in resources.)
    public string iconName = "icon_blank";

    public override void OnInitialise()
    {
        base.OnInitialise();
        AddVar("onGround", true);
    }

    /// <summary>
    /// Standard initialization function called by everyone.
    /// </summary>
    protected override void ClientInit()
    {
        viewTransform = GetComponent<PhotonTransformView>();
    }

    /// <summary>
    /// Fall to the ground!
    /// </summary>
    protected override void OwnerUpdate()
    {
        if(PhotonNetwork.IsMasterClient && GetVar<bool>("onGround"))
        {
            RaycastHit hit;
            int layermask = 1 << 0;
            if(Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), -transform.up, out hit, 100, layermask))
            {
                if(hit.distance > 0.1f)
                {
                    transform.Translate(new Vector3(0, -Mathf.Min(Time.deltaTime * itemFallSpeed, hit.distance), 0));
                }
            }
            else
            {
                transform.Translate(new Vector3(0, -Time.deltaTime * itemFallSpeed, 0));
                if(transform.position.y < -50)
                {
                    //Item has just fallen out the bottom of the world, get rid of it.
                    PhotonNetwork.Destroy(photonView);
                }
            }
        }
    }

    /// <summary>
    /// Must be called by the owner.
    /// Causes the transform view to stop updating (when its in hand).
    /// </summary>
    protected void StopTransformUpdates()
    {
        if(!photonView.IsMine)
        {
            Log.PrintDebug("Error: Cannot stop transforms on something we don't own.");
            return;
        }
        viewTransform.m_SynchronizePosition = false;
        viewTransform.m_SynchronizeRotation = false;
    }

    /// <summary>
    /// Must be called by the owner.
    /// Causes the transform view to start updating (when its on ground).
    /// </summary>
    protected void StartTransformUpdates()
    {
        if(!photonView.IsMine)
        {
            Log.PrintDebug("Error: Cannot stop transforms on something we don't own.");
            return;
        }
        viewTransform.m_SynchronizePosition = true;
        viewTransform.m_SynchronizeRotation = true;
    }

}
