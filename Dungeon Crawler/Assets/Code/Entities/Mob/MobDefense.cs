using Dungeon_Crawler.Assets.Code.UserInterface.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mob : Entity
{

    //Getter to see how much health we have remaining
    public int healthLeft
    {
        get { return maxHealth - damageLoss; }
    }

    //The maximum health of the mob.
    protected int maxHealth { get; set; } = 1;

    //Damage losses.
    protected int damageLoss = 0;

    //Armour of the mob
    protected Armour armour = new Armour();

    /// <summary>
    /// Sets the armour of the mob. Usually done on init.
    /// </summary>
    /// <param name="newArmour"></param>
    public void SetArmour(Armour newArmour)
    {
        armour = newArmour;
    }

    /// <summary>
    /// Applies damage to the mob.
    /// Pretty insecure to be honest.
    /// </summary>
    public void ApplyDamage(DamageType damageType, int force, int armourPenetration)
    {
        if(photonView.IsMine)
        {
            RPCDealDamage(damageType, force, armourPenetration);
        }
        else
        {
            photonView.RPC("RPCDealDamage", photonView.Controller, damageType, force, armourPenetration);
        }
    }

    /// <summary>
    /// Very unsafe, since hackers can just call it on anyone.
    /// Not too much of a worry right now, intended that they just get kicked by host.
    /// </summary>
    [PunRPC]
    public void RPCDealDamage(DamageType damageType, int force, int armourPenetration)
    {
        damageLoss -= armour.GetDamageAfterArmour(damageType, force, armourPenetration);
    }

}
