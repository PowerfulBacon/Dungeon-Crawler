using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the individual data for each tile
/// This would be a monobehaviour if it wasn't for the mesh combining optimisation
/// </summary>
public class Turf
{

    public Turf(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    //Positional values
    public int x { get; private set; }
    public int y { get; private set; }

    //For debug
    public bool door = false;

    //Used for level generation
    public bool calculated = false;

    //Determines if a floor tile is present (alternative is a hole)
    public bool hasFloor = true;
    //Determines if a ceiling is present (alternative is sky)
    public bool hasCeiling = true;

    //Tile rendering + calculations
    public bool occupied = false;
    public Mesh turfMesh;

}
