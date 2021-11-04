using System.Collections;
using System.Collections.Generic;
using Dungeon_Crawler;
using UnityEngine;

public partial class Mob : Entity
{

    protected Turf targetLocation;
    protected Entity targetEntity;

    //If true will pause requesting to find paths until we get the one we need
    protected bool awaitingPath = false;
    //The path we are currently moving upon
    protected Path path;
    
    /// <summary>
    /// To be run by the master client.
    /// </summary>
    public virtual void HandleMobAction()
    {
        //Firstly check our vision
        CheckMobVision();
        
        if(targetEntity == null)
        {
            if(GetMobTarget())
            {
                MoveToTarget();
            }
            else
            {
                Wander();
            }
        }
        else
        {
            MoveToTarget();
        }
    }

    /// <summary>
    /// Gets a nearby mob target
    /// </summary>
    /// <returns>True if we find a nearby target</returns>
    public virtual bool GetMobTarget()
    {
        return false;
    }

    /// <summary>
    /// Move to target location, or just randomly move if its null
    /// </summary>
    public virtual void Wander()
    { }

    /// <summary>
    /// Move towards our target.
    /// </summary>
    public virtual void MoveToTarget()
    {

    }

    /// <summary>
    /// Checks to see if we can see the target and makes them null if we cannot.
    /// </summary>
    public virtual void CheckMobVision()
    {
        if(targetEntity == null) return;
        //Linecast ignoring mobs
        int layerMask = (1 << 0);
        if(Physics.Linecast(transform.position, targetEntity.transform.position, layerMask))
        {
            targetEntity = null;
        }
    }

}
