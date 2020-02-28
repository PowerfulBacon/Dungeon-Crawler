using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Runtime.CompilerServices;
using System.Linq;

public class LevelGenerator : Subsystem
{

    public static int currentSeed;

    Resource levelResources;

    MonoLevelMaster levelMaster;

    GameObject prefabObject = null;

    public override void Initialise()
    {
        //Load the level resources
        levelResources = new Resource(Directories.DIR_LEVELRESOURCES);
    }


    protected override void Update()
    {

        if (subsystemQuery.Keys.Count <= 0)
            return;

        //Get the query
        string queryName = subsystemQuery.Keys.ElementAt(0);
        object queryData = subsystemQuery[queryName];

        //Execute
        switch (queryName)
        {
            case "generateLevel":
                GenerateLevel((int)((object[])queryData)[0], (int)((object[])queryData)[1]);
                break;
        }

    }


    public Level GenerateLevel(int seed, int levelSize = 255)
    {

        Debug.Log("Generating this might take a while for large levels...");

        if (prefabObject == null)
        {
            SetupPrefabObject();
        }

        //Create level holder thingy
        Level level = new Level(levelSize, levelSize);

        //Set the seed
        Random.InitState(seed);

        //Generate blank level
        for (int x = 0; x < levelSize; x++)
        {
            for (int y = 0; y < levelSize; y++)
            {
                //Generate Forced Layers (Floor + Ceiling)
                //THIS
                //IS WAY
                //TOO
                //SLOW
                //STORE NORMALLY IN ARRAY
                //IN GAME OBJECTES, MUST COMBINE MESHES FOR EFFICIENCY
                GameObject floorLayer = Object.Instantiate(prefabObject, new Vector3(x, 0, y), Quaternion.identity);
                level.turfs[x, y, 0] = floorLayer.GetComponent<Turf>();
            }
        }

        return null;

    }


    public void SetupPrefabObject()
    {
        prefabObject = (GameObject)levelResources.loadedResources["turf"];
    }

}
