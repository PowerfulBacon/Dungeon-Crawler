using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceComponent
{

    public Vector2 position;
    public Vector2 size;

    public Vector2 collisionSize;

    public List<MonoBehaviour> components = new List<MonoBehaviour>();

    public List<UserInterfaceComponent> children = new List<UserInterfaceComponent>();

    /// <summary>
    /// Used to generate the unity components that get put on the canvas
    /// </summary>
    public virtual void GenerateComponents()
    {
        
    }

    /// <summary>
    /// On update
    /// </summary>
    public virtual void UpdateComponent()
    {
        //Update children components
        foreach(UserInterfaceComponent child in children)
        {
            child.UpdateComponent();
        }
    }

}
