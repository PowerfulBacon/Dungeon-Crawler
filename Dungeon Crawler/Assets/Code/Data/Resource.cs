using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{

    public string resourceLocation;

    public Dictionary<string, object> loadedResources = new Dictionary<string, object>();

    public Resource(string location )
    {
        this.resourceLocation = location;
        LoadResources();
    }

    public void LoadResources()
    {
        //Load the resources
        UnityEngine.Object[] rawResources = Resources.LoadAll(resourceLocation);

        //Add to dictionary
        foreach(UnityEngine.Object resource in rawResources)
        {
            try
            {
                loadedResources.Add(resource.name, resource);
            }
            catch(Exception e)
            {
                Debug.LogError("Major error while loading, " + e.Message);
            }
        }
    }

}
