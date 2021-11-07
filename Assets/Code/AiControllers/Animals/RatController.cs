using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : AiControllerDefault
{

    private enum Action
    {
        Attack,
        Flee,
        Wander,
    }

    public override IMobAi target { get; set; }

    public override int visionRange { get; } = 5;

    private float fleeMultiplier = 0.4f;

    public override void CheckVision(List<IMobAi> view)
    {
        
    }

    public override void CheckTarget()
    {
        
    }

    public override void PerformAction()
    {

        //The mob is busy
        if(parent.IsMobBusy())
        {
            return;
        }

        switch(DecideAction())
        {
            case Action.Attack:
                HandleAttackAction();
                break;
            case Action.Flee:
                HandleFleeAction();
                break;
            case Action.Wander:
                HandleWanderAction();
                break;
        }
    }

    //Walk towards the target if not near them
    //Attack the target when in range after some small time
    private void HandleAttackAction()
    {
        if(parent.IsInAttackRange(target))
        {
            parent.DoGenericAttack(target);
        }
        else
        {
            parent.MoveTowards(target.GetPosition());
        }
    }

    private void HandleFleeAction()
    {

    }

    private void HandleWanderAction()
    {

    }

    private Action DecideAction()
    {
        if(target == null)
            return Action.Wander;
        if(parent.GetHealth() < parent.GetMaxHealth() * fleeMultiplier)
            return Action.Flee;
        return Action.Attack;
    }

}
