using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{

    //Same for all spells
    public int manaCost { get; set; }           //How much mana it costs to cast.
    public float cooldownTime { get; set; }     //Cooldown time in seconds

    //Instantiated for this object
    public float worldCooldownTime { get; set; }

    //"Icons/{inventoryIcon}"
    public string inventoryIcon = "";

    /// <summary>
    /// Casts if possible.
    /// Called on the server, requested by the mob
    /// </summary>
    public virtual void Cast(Mob caster)
    {
        if(!CanCast())
            return;
        BeginRecharge();
        DoCast(caster.transform.position);
    }

    /// <summary>
    /// Do the cast effect
    /// </summary>
    public virtual void DoCast(Vector3 aimLocation)
    { }

    /// <summary>
    /// Set the recharge time
    /// </summary>
    public virtual void BeginRecharge()
    {
        worldCooldownTime = Time.time + cooldownTime;
    }

    /// <summary>
    /// Check if we can cast the spell
    /// </summary>
    /// <returns></returns>
    public virtual bool CanCast()
    {
        if(Time.time > worldCooldownTime) return false;
        return true;
    }

}
