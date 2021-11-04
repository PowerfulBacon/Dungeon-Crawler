using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dummy item that can only exist in the inventory.
/// Exists as a way for users to equip spells and cannot be dropped.
/// </summary>
public class SpellHolder : Item
{
    
    protected override void ClientInit()
    {
        //No dropping :(
        flags = FLAG_DROP_DEL;
    }

}
