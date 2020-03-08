using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{

    public static Material globalMaterial;

    public Mesh itemMesh;
    public Sprite itemIcon;

    public virtual void OnInitialise() { }

    public virtual void OnUpdate() { }

    public virtual void OnUse() { }

    public virtual void Drop(Mob m)
    {

    }

    public virtual void Throw(Mob m)
    {
        
    }

}
