using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Master : MonoBehaviour
{

    public static SubsystemMasterMono subsystemMaster = null;
    public static Dictionary<string, Subsystem> subsystems = new Dictionary<string, Subsystem>();

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
        subsystems.Add("levelGeneration", new LevelGenerator());
        subsystems.Add("networkManagement", new NetworkMaster());
    }

}
