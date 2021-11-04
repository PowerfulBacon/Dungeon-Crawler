using System;
using System.Collections.Generic;
using System.Text;

public class Node
{

    //I don't really get why they need getter and setters when they can be public, but its a good practise.
    public int x { get; set; }
    public int y { get; set; }

    //=== Calculated by the world and updates to the world ===

    //True if this node cannot be traversed.
    bool blocked;
    //The cost multiplier of traversing through this node.
    //Literally not used atm lol (Just remove if you dont add it)
    float costMultiplier;

    //=== Calculated Dynamically Per Path ===

    //Have we been checked
    public bool isChecked { get; set; } = false;
    //The pathID of the last calc
    public int pathId { get; set; }
    //The cost of the best found path to the start
    public float gCost { get; set; }
    //The distance in world units to the exit
    public float hCost { get; set; }
    //The total cost of examining this node
    float totalCost;
    //Where do we originate from
    public Node originNode { get; set; }

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Sets the gCost and hCost.
    /// </summary>
    /// <param name="gCost"></param>
    /// <param name="hCost"></param>
    public void SetCosts(float gCost, float hCost)
    {
        this.gCost = gCost;
        this.hCost = hCost;
        totalCost = gCost + hCost;
    }

    /// <summary>
    /// Gets the total cost of examining this node.
    /// </summary>
    public float GetTotalCost()
    {
        return totalCost;
    }

    public void SetBlocked(bool isBlocked)
    {
        blocked = isBlocked;
    }

    public bool GetBlocked()
    {
        return blocked;
    }

    /// <summary>
    /// Recursive function that gets the shortest known route to the origin node.
    /// Pretty cool.
    /// </summary>
    /// <returns></returns>
    public List<Node> GetRecursiveNodes()
    {
        //Console.WriteLine($"getting parent of {x}, {y}");
        List<Node> parentNodes = originNode != null
            ? originNode.GetRecursiveNodes()
            : new List<Node>();
        parentNodes.Add(this);
        return parentNodes;
    }

}

