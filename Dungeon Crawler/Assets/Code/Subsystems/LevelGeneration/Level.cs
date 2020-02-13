using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{

    public Turf[,] turfs;

    public Level(int xSize, int ySize)
    {
        turfs = new Turf[xSize, ySize];
    }

}
