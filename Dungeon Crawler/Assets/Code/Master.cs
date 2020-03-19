//Enables or disables debug mode
#define DEBUG
//#undef DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

        //Load all subsystems
        LoadAllSubsystems();
        ExecuteSubsystems();

    }


    public static void ExecuteSubsystems()
    {
        GameObject master = new GameObject("master");
        subsystemMaster = master.AddComponent<SubsystemMasterMono>();
    }


    public static void LoadAllSubsystems()
    {
        subsystems.Add("levelGeneration", new LevelGenerator("levelGenerator"));
        subsystems.Add("networkManagement", new NetworkMaster("networkMaster"));
        subsystems.Add("entities", new EntitySubsystem("entities"));
    }

}
