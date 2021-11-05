using System.Collections;
using System.Collections.Generic;
using Dungeon_Crawler;
using UnityEngine;

public partial class Mob : Entity
{

    protected virtual IAiController aiController { get; set; }
    
    public virtual float attackRange { get; } = 0.3f;
    protected virtual DamageType genericAttackDamageType { get; } = DamageType.Blunt;
    protected virtual int genericAttackDamageAmount { get; } = 1;
    protected virtual int genericAttackArmourPenetration { get; } = 0;

    protected virtual void SetupAiController()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// To be run by the master client.
    /// </summary>
    public virtual void HandleMobAction()
    {
        aiController.PerformAction();
    }

    /**
     * Begins the attack animation:
     *  - Starts a co-routine that:
     *  - Waits for a small time
     *  - Applies damage to the target if they are still in range (Calls the damage method)
     *  - Does the animation for attack
     */
    public virtual void DoGenericAttack(Mob target)
    {
        StartCoroutine("AttackCoroutine", target);
    }

    /**
     * Do the attack animatino and apply damage.
     * Animation:
     * Mob moves backwards slowly
     * Mob moves forward quickly and applies damage if target still in range
     * Mob shakes a bit.
     */
    protected virtual IEnumerator GenericAttackCoroutine(Mob target)
    {
        //TODO
        return null;
    }

    protected virtual void DoGenericAttackDamage(Mob target, Vector3 damamgeLocation)
    {
        target.ApplyDamage(genericAttackDamageType, genericAttackDamageAmount, genericAttackArmourPenetration);
    }

    public bool CheckFactions(Mob other)
    {
        foreach(string faction in factions)
        {
            if(other.factions.Contains(faction))
                return true;
        }
        return false;
    }

}
