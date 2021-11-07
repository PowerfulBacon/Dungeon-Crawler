using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AiControllerDefault : IAiController
{
    
    public IMobAi parent { get; set; }

    public virtual IMobAi target { get; set; }

    public virtual int visionRange { get; } = 3;

    //If we are forgetting the target already, ignore the checktarget check
    private bool forgettingTarget = false;
    //The layermask to use when raycasting
    private int layermask = ~(1 << 10);

    public virtual void CheckTarget()
    {
        if(forgettingTarget)
            return;
        //Raycast to target and check if they are still in view
        if(!Physics.Linecast(target.GetPosition(), parent.GetPosition(), layermask))
        {
            //Check again and forget after a few seconds
            Thread thread = new Thread(CheckAndForgetTarget);
            thread.Start();
            forgettingTarget = true;
        }
    }

    public virtual void CheckVision(List<IMobAi> view)
    {
        //Try to find new targets.
        //Get all targets within the vision range
        foreach (IMobAi mob in view)
        {
            //Ignore friendly mobs
            if(parent.CheckFactions(mob))
                continue;
            //Ignore mobs that are out of our vision range.
            if(Vector3.Distance(mob.GetPosition(), parent.GetPosition()) > visionRange)
                continue;
            //Ignore mobs that are obstructed by walls or whatever
            if(!Physics.Linecast(mob.GetPosition(), parent.GetPosition(), layermask))
                continue;
            //New target found
            target = mob;
            return;
        }
    }

    //Executed on a seperate thread
    private void CheckAndForgetTarget()
    {
        //I think this works.
        Thread.Sleep(1000);
        if(!Physics.Linecast(target.GetPosition(), parent.GetPosition(), layermask))
        {
            ForgetTarget();
        }
        forgettingTarget = false;
    }

    public void ForgetTarget()
    {
        //Forget about the current target
        target = null;
    }

    public virtual void PerformAction()
    {
        throw new System.NotImplementedException();
    }

    public void Takeover(IMobAi parent)
    {
        this.parent = parent;
    }
}
