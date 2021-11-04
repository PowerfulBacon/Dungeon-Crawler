using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemode
{
    public string name = "";

    //Settings that will shot up in the menu
    public Dictionary<string, object> settings = new Dictionary<string, object>();

    public virtual void OnStart()
    {
        
    }

}
