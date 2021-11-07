
//Generic mob AI interface
using System.Collections.Generic;
using UnityEngine;

/**
 * An interface that provides actions that can be used
 * by Ai controllers.
 */
public interface IMobAi
{

    //Returns the mob we are attached to
    Mob GetParent();

    //Returns the position of the mob
    Vector3 GetPosition();

    //Returns the health of the mob
    int GetHealth();

    //Returns the max health of the mob
    int GetMaxHealth();

    //Returns the factions of the mob
    List<string> GetFactions();

    //Returns true if the mob is doing something that prevents
    //the AI from updating.
    bool IsMobBusy();

    //Returns true if the mob is in attack range.
    bool IsInAttackRange(IMobAi target);

    //Mob moves towards a point
    void MoveTowards(Vector3 point);

    //Performs a generic attack targetted at the target mob.
    void DoGenericAttack(IMobAi target);

    //Checks factional relation between 2 mobs
    //Returns true if the mobs are in shared factions
    bool CheckFactions(IMobAi other);

}
