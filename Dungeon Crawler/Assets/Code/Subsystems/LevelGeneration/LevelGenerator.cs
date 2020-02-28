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

    protected override void Update()
    {
        base.Update();
    }

    public Level GenerateLevel(int seed, int levelSize = 255)
    {

        Random.InitState(seed);

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
