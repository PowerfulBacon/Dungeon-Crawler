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

    GameObject tempObject;

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

        subsystemQuery.Remove(queryName);

    }


    public Level GenerateLevel(int seed, int levelSize = 255)
    {

        Debug.Log("Generating this might take a while for large levels...");

        //Create level holder thingy
        Level level = new Level(levelSize, levelSize);

        //Set the seed
        Random.InitState(seed);

        //Get the mesh
        MeshFilter objectMesh = ((GameObject)levelResources.loadedResources["turf"]).GetComponentInChildren<MeshFilter>();

        const int MAX_VERTEX_COUNT = 65536;
        int currentVertexCount = 0;

        tempObject = Object.Instantiate((GameObject)levelResources.loadedResources["turf"], new Vector3(0, 0, 0), Quaternion.identity);
        tempObject.SetActive(false);

        //Generate the walls and rooms and shit
        var activeLayer = GenerateActiveLayer(levelSize);

        for (int x = 0; x < levelSize; x++)
        {
            for (int y = 0; y < levelSize; y++)
            {
                if (currentVertexCount + objectMesh.sharedMesh.vertexCount * 2 >= MAX_VERTEX_COUNT)
                {
                    GenerateCombinedMeshes(currentVertexCount);
                    currentVertexCount = 0;
                }

                //Load the cube
                GameObject tempChild = new GameObject();
                tempChild.transform.SetParent(tempObject.transform);
                var f = tempChild.AddComponent<MeshFilter>();
                f.sharedMesh = objectMesh.sharedMesh;
                f.transform.position = new Vector3(x * 2, 0, y * 2);
                f.transform.localScale = new Vector3(2, 2, 2);
                currentVertexCount += objectMesh.sharedMesh.vertexCount;

                //Load the cube
                if (activeLayer[x, y].occupied)
                {
                    GameObject middleLayer = new GameObject();
                    middleLayer.transform.SetParent(tempObject.transform);
                    var middleFilter = middleLayer.AddComponent<MeshFilter>();
                    middleFilter.sharedMesh = objectMesh.sharedMesh;
                    middleFilter.transform.position = new Vector3(x * 2, 1 * 2, y * 2);
                    middleFilter.transform.localScale = new Vector3(2, 2, 2);
                    currentVertexCount += objectMesh.sharedMesh.vertexCount;
                }
            }
        }

        GenerateCombinedMeshes(currentVertexCount);

        return null;

    }


    public List<GenerationAreaSettings> ReadGenerationSettings()
    {
        Resource resource = new Resource("Data");
        List<GenerationAreaSettings> areas = LevelGenDataParser.ParseGenerationDataJson(resource);
        return areas;
    }


    public Turf[,] GenerateActiveLayer(int size = 255)
    {

        MeshFilter objectMesh = ((GameObject)levelResources.loadedResources["turf"]).GetComponentInChildren<MeshFilter>();

        List<GenerationAreaSettings> areaData = ReadGenerationSettings();
        Turf[,] turfs = new Turf[size, size];

        //Count how many rooms were made
        int roomCount = 0;
        int successfulCount = 0;

        //Populate array with default values
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y ++)
                turfs[x, y] = new Turf();

        List<TempTileData> tilesToProcess = new List<TempTileData>();
        TempTileData initialRoom = new TempTileData();
        initialRoom.connection_x = 1;
        initialRoom.connection_y = 0;
        initialRoom.x = size / 2;
        initialRoom.y = size / 2;
        tilesToProcess.Add(initialRoom);

        //Generate the rooms
        while (tilesToProcess.Count > 0)
        {
            //Log the amount of rooms
            roomCount++;
            Debug.Log("Processing tile at " + tilesToProcess[0].x + "," + tilesToProcess[0].y + " with direction " + tilesToProcess[0].connection_x + "," + tilesToProcess[0].connection_y);

            //Generate a list of rooms that can be used
            List<GenerationAreaSettings> validRooms = new List<GenerationAreaSettings>();

            //Check which areas can be placed
            foreach (GenerationAreaSettings setting in areaData)
            {
                Direction direction = Direction.NORTH;
                if (tilesToProcess[0].connection_x == 1) direction = Direction.WEST;
                else if (tilesToProcess[0].connection_x == -1) direction = Direction.EAST;
                else if (tilesToProcess[0].connection_y == 1) direction = Direction.NORTH;
                else if (tilesToProcess[0].connection_y == -1) direction = Direction.SOUTH;
                else Debug.LogError("Major error, door direction invalid");
                validRooms.AddRange(setting.CheckSpace(turfs, tilesToProcess[0].x, tilesToProcess[0].y, direction));
            }

            //Check weights
            //Place it
            if (validRooms.Count == 0)
            {
                Debug.LogError("WARNGING, NO VALID ROOMS TO PLACE :FLUSHED:");
                tilesToProcess.RemoveAt(0);
                continue;
            }

            GenerationAreaSettings chosenArea = Helpers.Pick(validRooms);

            foreach (GenerationTurfSettings tile in chosenArea.generationTurfSettings)
            {
                //Debug.Log("[" + tile.x + "]" + "[" + tile.y + "] : Added turf, type : [" + tile.type + "]" + (tile.type == "wall").ToString()); //DEBUG, REMOVE WHEN DONE
                turfs[tile.x, tile.y].calculated = true;
                turfs[tile.x, tile.y].occupied = tile.type == "wall";
                turfs[tile.x, tile.y].turfMesh = objectMesh.sharedMesh;
                if (tile.door_dir_x != 0 || tile.door_dir_y != 0)
                    turfs[tile.x, tile.y].door = true;

                if (tile.door_dir_x != 0 || tile.door_dir_y != 0)
                {
                    TempTileData data = new TempTileData();
                    data.x = tile.x;
                    data.y = tile.y;
                    data.connection_x = tile.door_dir_x;
                    data.connection_y = tile.door_dir_y;
                    tilesToProcess.Add(data);

                    Debug.Log("Added new tile to be processed at " + data.x + "," + data.y + "," + data.connection_x + "," + data.connection_y);
                }

            }

            successfulCount++;
            tilesToProcess.RemoveAt(0);

        }

        Debug.Log("<color=green>Successfully generated " + roomCount + " rooms, of which " + successfulCount + " were successful</color>");

        return turfs;
    }


    public void GenerateCombinedMeshes(int currentVertexCount)
    {
        MeshFilter[] meshFilters = tempObject.GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> meshes = new List<CombineInstance>();

        for (int i = 1; i < meshFilters.Length; i++)
        {
            CombineInstance instance = new CombineInstance();
            instance.mesh = meshFilters[i].sharedMesh;
            instance.transform = meshFilters[i].transform.localToWorldMatrix;
            meshes.Add(instance);
        }

        GameObject holdStuff = new GameObject("MeshObject");
        holdStuff.AddComponent<MeshFilter>();
        holdStuff.AddComponent<MeshRenderer>();
        holdStuff.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        holdStuff.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(meshes.ToArray(), true);
        holdStuff.GetComponent<MeshRenderer>().materials = ((GameObject)levelResources.loadedResources["turf"]).GetComponent<MeshRenderer>().sharedMaterials;
        holdStuff.AddComponent<MeshCollider>();
        holdStuff.SetActive(true);

        Object.Destroy(tempObject);
        tempObject = Object.Instantiate((GameObject)levelResources.loadedResources["turf"], new Vector3(0, 0, 0), Quaternion.identity);
        tempObject.SetActive(false);
    }

}
