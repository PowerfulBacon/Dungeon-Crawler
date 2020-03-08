using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the individual data for each tile
/// This would be a monobehaviour if it wasn't for the mesh combining optimisation
/// </summary>
public class Turf
{

    public Turf(){}

    //For debug
    public bool door = false;

    //Used for level generation
    public bool calculated = false;

    //Tile rendering + calculations
    public bool occupied = false;
    public Mesh turfMesh;

}
