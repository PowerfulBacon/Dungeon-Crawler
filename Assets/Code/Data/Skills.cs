using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skills
{

    public Dictionary<string, float> levelCaps = new Dictionary<string, float>
    {
        { "None", 0 },
        { "Basic", 50 },
        { "Novice", 200 },
        { "Experienced", 400 },
        { "Expert", 1000 },
        { "Master", 2000 },
        { "Legendary", 5000 }
    };

    //Combat
    public float experience_armed_melee;    //Armed combat
    public float experience_unarmed_melee;  //Unarmed combat including disarming
    public float experience_casting;        //Casting spells
    public float experience_magic;          //Using magical items
    public float experience_ranged;         //Ranged items
    public float experience_shields;        //Blocking attacks

    //Actions
    public float experience_examine;        //Examining item properties
    public float experience_enchanting;     //Enchanting items
    public float experience_trading;        //Trading items with NPCs
    public float experience_picking;        //Picking things

    public int GetLevel(float experience)
    {
        for (int i = 0; i < levelCaps.Values.Count; i++)
        {
            if (experience < levelCaps.Values.ElementAt(i))
                return Mathf.Clamp(i - 1, 0, levelCaps.Count - 1);
        }
        return levelCaps.Count - 1;
    }

}
