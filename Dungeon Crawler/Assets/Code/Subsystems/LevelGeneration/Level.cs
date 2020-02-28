using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{

    //The amount of z layers there are
    //Ground
    //Walls
    //Ceiling
    //Raised ceiling
    public const int Z_LAYERS = 4;

    public Turf[,,] turfs;

    public Level(int xSize, int ySize)
    {
        turfs = new Turf[xSize, ySize, Z_LAYERS];
    }

}
