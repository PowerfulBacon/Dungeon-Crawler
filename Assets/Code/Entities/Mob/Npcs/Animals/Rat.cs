using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Mob
{
    
    //Rats are part of the rat faction
    public override List<string> factions { get; } = new List<string>() { "rat" };

    protected override void SetupAiController()
    {
        aiController = new RatController();
        aiController.Takeover(this);
    }

}
