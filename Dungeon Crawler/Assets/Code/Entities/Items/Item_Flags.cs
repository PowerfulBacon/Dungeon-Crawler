using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anything that can be put in the inventory ---MUST--- inherit this :) <3
/// </summary>
public partial class Item : Entity
{

    public const int FLAG_DROP_DEL = 1 << 0;    //Delete instead of dropping

    public int flags = 0;

}
