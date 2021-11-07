using System.Collections;
using System.Collections.Generic;
using Dungeon_Crawler;
using UnityEngine;

/**
 * All AI related actions
 */
public partial class Mob : Entity, IMobAi
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

    //Mob moves towards a point
    public void MoveTowards(Vector3 point)
    {
        throw new System.NotImplementedException();
    }

    /**
     * Begins the attack animation:
     *  - Starts a co-routine that:
     *  - Waits for a small time
     *  - Applies damage to the target if they are still in range (Calls the damage method)
     *  - Does the animation for attack
     */
    public virtual void DoGenericAttack(IMobAi target)
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
    protected virtual IEnumerator GenericAttackCoroutine(IMobAi target)
    {
        //The mob should be frozen while doing this.
        Vector3 startingPosition = transform.position;
        float timeElapsed = 0;
        //Move backwards
        while(timeElapsed < 0.25f)
        {
            timeElapsed += 1 / 30.0f;
            transform.position = startingPosition + (-0.2f * transform.forward) * Mathf.Sin(timeElapsed * (Mathf.PI * 2 / 0.25f));
            yield return new WaitForSeconds(1 / 30.0f);
        }
        //Lunge forward
        timeElapsed = 0;
        while(timeElapsed < 0.05f)
        {
            timeElapsed += 1 / 30.0f;
            transform.position = (startingPosition - 0.2f * transform.forward) + (0.4f * transform.forward) * (timeElapsed / 0.05f);
            yield return new WaitForSeconds(1 / 30.0f);
        }
        //Deal damage (if player is still in range)
        if(Vector3.Distance(target.GetPosition(), transform.position) < attackRange)
        {
            DoGenericAttackDamage(target, target.GetPosition());
        }
    }

    protected virtual void DoGenericAttackDamage(IMobAi target, Vector3 damamgeLocation)
    {
        target.GetParent().ApplyDamage(genericAttackDamageType, genericAttackDamageAmount, genericAttackArmourPenetration);
    }

    public bool CheckFactions(IMobAi other)
    {
        foreach(string faction in factions)
        {
            if(other.GetFactions().Contains(faction))
                return true;
        }
        return false;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public int GetHealth()
    {
        return healthLeft;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public List<string> GetFactions()
    {
        return factions;
    }

    public Mob GetParent()
    {
        return this;
    }

    public bool IsMobBusy()
    {
        throw new System.NotImplementedException();
    }

}
