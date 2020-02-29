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
        Debug.Log(objectMesh);

        const int MAX_VERTEX_COUNT = 65536;
        int currentVertexCount = 0;

        tempObject = Object.Instantiate((GameObject)levelResources.loadedResources["turf"], new Vector3(0, 0, 0), Quaternion.identity);
        tempObject.SetActive(false);

        for (int x = 0; x < levelSize; x++)
        {
            for (int y = 0; y < levelSize; y++)
            {
                //<overview>
                //Generate Forced Layers (Floor + Ceiling)
                //THIS
                //IS WAY
                //TOO
                //SLOW
                //STORE NORMALLY IN ARRAY
                //IN GAME OBJECTES, MUST COMBINE MESHES FOR EFFICIENCY
                //</overview>

                if (currentVertexCount + objectMesh.sharedMesh.vertexCount >= MAX_VERTEX_COUNT)
                {
                    GenerateCombinedMeshes(currentVertexCount);
                    currentVertexCount = 0;
                }

                //Load the cube
                GameObject tempChild = new GameObject();
                tempChild.transform.SetParent(tempObject.transform);
                var f = tempChild.AddComponent<MeshFilter>();
                f.sharedMesh = objectMesh.sharedMesh;
                f.transform.position = new Vector3(x, 0, y);
                f.transform.localScale = new Vector3(1, 1, 1);

                currentVertexCount += objectMesh.sharedMesh.vertexCount;
            }
        }

        GenerateCombinedMeshes(currentVertexCount);

        return null;

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
