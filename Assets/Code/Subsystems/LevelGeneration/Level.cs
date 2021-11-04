using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{

    public Turf[,] turfs;
    public int levelSize;

    public Level(int levelSize)
    {
        this.levelSize = levelSize;
        turfs = new Turf[levelSize, levelSize];
    }

}
