using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct StatModifier
{
    public float stength_multiplier;
    public float dexterity_multiplier;
    public float constitution_multiplier;
    public float intelligence_multiplier;
    public float wisdom_multiplier;
    public float charisma_multiplier;

    public float stength_increase;
    public float dexterity_increase;
    public float constitution_increase;
    public float intelligence_increase;
    public float wisdom_increase;
    public float charisma_increase;
}


public class Stats
{

    public float stength;
    public float dexterity;
    public float constitution;
    public float intelligence;
    public float wisdom;
    public float charisma;

    public Stats(Stats stats = null)
    {
        if (stats == null)
            return;
        stength      = stats.stength;
        dexterity    = stats.dexterity;
        constitution = stats.constitution;
        intelligence = stats.intelligence;
        wisdom       = stats.wisdom;
        charisma     = stats.charisma;
    }

    /// <summary>
    /// Get the stats after modification (item modifiers)
    /// 
    /// </summary>
    /// <param name="statModifiers"></param>
    /// <returns></returns>
    public Stats CalculateModifiedStats(StatModifier[] statModifiers)
    {
        Stats stats = new Stats(this);

        foreach (StatModifier multiplier in statModifiers)
        {
            stats.stength       *= multiplier.stength_multiplier;
            stats.dexterity     *= multiplier.dexterity_multiplier;
            stats.constitution  *= multiplier.constitution_multiplier;
            stats.intelligence  *= multiplier.intelligence_multiplier;
            stats.wisdom        *= multiplier.wisdom_multiplier;
            stats.charisma      *= multiplier.charisma_multiplier;
        }

        foreach (StatModifier incrementer in statModifiers)
        {
            stats.stength       *= incrementer.stength_increase;
            stats.dexterity     *= incrementer.dexterity_increase;
            stats.constitution  *= incrementer.constitution_increase;
            stats.intelligence  *= incrementer.intelligence_increase;
            stats.wisdom        *= incrementer.wisdom_increase;
            stats.charisma      *= incrementer.charisma_increase;
        }

        return stats;
    }

    //Take weight into account
    //Make final go through a scale that prevents it from being ridiculous
    public float GetSpeed(Mob m)
    {
        return dexterity * 0.4f + 3.5f;
    }

}
