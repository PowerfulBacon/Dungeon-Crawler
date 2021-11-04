using System.Collections;
using System.Collections.Generic;
using Dungeon_Crawler.Assets.Code.UserInterface.Chat;
using UnityEngine;

public partial class Item : Entity
{

    public virtual void ExamineItem()
    {
        Chat.ToChat($"<style=notice>That's {GetPronoun()} {itemName}!\n{description}</style>");
    }

    private string GetPronoun()
    {
        switch(itemName.Substring(0, 1))
        {
            case "a":
            case "e":
            case "i":
            case "o":
            case "u":
                return "an";
            default:
                return "a";
        }
    }
    
}
