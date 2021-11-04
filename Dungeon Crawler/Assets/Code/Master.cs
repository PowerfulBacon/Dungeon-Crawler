//Enables or disables debug mode
#define DEBUG
//#undef DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 * The thing that creates everything
 */
public class Master : MonoBehaviour
{

    public static SubsystemMasterMono subsystemMaster = null;
    public static Dictionary<string, Subsystem> subsystems = new Dictionary<string, Subsystem>();

    //If we are running in debug mode
#if DEBUG
    public const bool DEBUG_MODE = true;
#else
    public const bool DEBUG_MODE = false;
#endif

    [RuntimeInitializeOnLoadMethod]
    public static void OnGameStart()
    {

        Log.ServerMessage("Game started!");

        //Load all subsystems
        LoadAllSubsystems();
        ExecuteSubsystems();

        //Load physics ignores
        //Mobs do not collide with objects on the ground.
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("GroundItem"), LayerMask.NameToLayer("Mob"), true);

    }


    public static void ExecuteSubsystems()
    {
        Log.ServerMessage("Creating master object.");
        GameObject master = new GameObject("master");
        subsystemMaster = master.AddComponent<SubsystemMasterMono>();
    }


    public static void LoadAllSubsystems()
    {
        subsystems.Add("levelGeneration", new LevelGenerator("levelGenerator"));
        subsystems.Add("networkManagement", new NetworkMaster("networkMaster"));
        subsystems.Add("entities", new EntitySubsystem("entities"));
        subsystems.Add("pathfinding", new Pathfinding("pathfinding"));
    }

}
