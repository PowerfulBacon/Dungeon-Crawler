using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEntity : Spell
{

    //The entity type that will be summoned    
    protected Type entityToSummon { get; set; }

    //Things we are summoned
    public Entity summonedEntity;

    public override void DoCast(Vector3 aimLocation)
    {
        if(summonedEntity != null)
        {
            summonedEntity.Destroy();
        }

        Entity.CreateEntity(entityToSummon, aimLocation, Quaternion.identity);

    }

}
