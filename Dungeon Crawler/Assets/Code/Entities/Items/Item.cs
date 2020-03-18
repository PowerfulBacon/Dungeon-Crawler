using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{

    public static Material globalMaterial;

    public Mesh itemMesh;
    public Sprite itemIcon;

    public virtual void OnInitialise() { }

    public virtual void OnUpdate() { }

    public virtual void OnUse() { }

    public virtual void OnPickup()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public virtual void Drop(Mob m)
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    public virtual void Throw(Mob m)
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

}
