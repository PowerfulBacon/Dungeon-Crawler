using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Master : MonoBehaviour
{

    public static List<Subsystem> subsystems = new List<Subsystem>();

    [RuntimeInitializeOnLoadMethod]
    public static void OnGameStart()
    {

        //Load all subsystems
        LoadAllSubsystems();

    }


    public static void LoadAllSubsystems()
    {
        subsystems.Add(new LevelGenerator());
    }

}
