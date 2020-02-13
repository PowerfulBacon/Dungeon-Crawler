using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Runtime.CompilerServices;

public class LevelGenerator : Subsystem
{

    MonoLevelMaster levelMaster;

    [PunRPC]
    public void RPCGenerateLevel(int seed, int levelSize = 255)
    {
        GenerateLevel(seed, levelSize);
    }

    public Level GenerateLevel(int seed, int levelSize = 255, [CallerMemberName] string callerMemberName = "?", [CallerLineNumber] int callerLineNumber = -1)
    {

        Random.InitState(seed);

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Attempted to generate level as non-server." + callerMemberName + ", line : " + callerLineNumber);
            return null;
        }

        //Generate blank level
        for (int x = 0; x < levelSize; x++)
        {
            for (int y = 0; y < levelSize; y++)
            {
                
            }
        }

        return null;

    }

}
