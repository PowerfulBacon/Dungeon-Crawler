using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates light and follows its summoner.
/// </summary>
public class WispEntity : Entity
{

    private Mob parent;

    protected override void SetModel()
    {
        model = "projectiles/lightproj";
    }

    public override void OnUpdate(float deltaTime)
    {
        //Parent stopped existing
        if(parent == null)
        {
            Destroy();
            return;
        }
        //Go to the mob with our speed based on distance.
        float distanceToMove = Vector3.Distance(parent.transform.position, transform.position);
        float movementSpeed = distanceToMove * distanceToMove * 0.4f * deltaTime;
        //Move
        transform.position = Vector3.MoveTowards(transform.position, parent.transform.position, movementSpeed);
    }

}
