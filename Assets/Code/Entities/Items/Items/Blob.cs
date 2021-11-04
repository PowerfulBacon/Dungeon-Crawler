using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anything that can be put in the inventory ---MUST--- inherit this :) <3
/// </summary>
public class Blob : Item
{

    protected override void SetModel()
    {
        itemName = "blobous mass";
        iconName = "blob";
        model = "blob";
        description = "An orange, blobous mass. It has large googly eyes, a mouth and some kind of reproductive organ.";
    }

}
