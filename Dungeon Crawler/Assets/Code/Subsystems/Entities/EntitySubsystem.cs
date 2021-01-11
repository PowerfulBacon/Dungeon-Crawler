using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EntitySubsystem : Subsystem
{

    public static List<Entity> entities = new List<Entity>();

    public EntitySubsystem(string name = "") : base(name)
    {
        
    }

    public override void Initialise()
    {
    }

    protected override void Update()
    {
        foreach (Entity entity in entities)
        {
            try
            {
                entity.OnUpdate();
            }
            catch(Exception e)
            {
                Log.PrintError("An error has occured, entity failed to update:\n" + e.Message, false);
            }
        }
    }

}
