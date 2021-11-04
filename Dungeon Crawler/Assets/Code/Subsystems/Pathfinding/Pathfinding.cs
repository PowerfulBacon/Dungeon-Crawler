using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Dungeon_Crawler;
using UnityEngine;

class Pathfinding : Subsystem
{

    public static Node[,] worldNodes;

    public Pathfinding(string name = "") : base(name)
    { }

    private static int pathId = 0;

    private List<PathfindingRequest> pathRequests = new List<PathfindingRequest>();

    protected override void Update(float processingTime)
    {
        
        if(subsystemQuery.Keys.Count <= 0)
        {
            //Run queued paths
            if(pathRequests.Count == 0)
                return;
            //Get the request
            PathfindingRequest request = pathRequests[0];
            pathRequests.RemoveAt(0);
            //Run the path
            CalculatePath(request);
            return;
        }
        
        //Get the query
        string queryName = subsystemQuery.Keys.ElementAt(0);
        object queryData = subsystemQuery[queryName];

        //Execute queries
        switch (queryName)
        {
            case "SetWorldConditions":
                SetWorldConditions();
                break;
            case "DebugPath":
                RequestDebugPath();
                break;
            default:
                Log.PrintError($"Unrecognised query [{queryName}] in networkGenerator, deleting");
                break;
        }

        subsystemQuery.Remove(queryName);
    }

    /// <summary>
    /// Sets the world conditions
    /// Useful when the world changes on mass (level load)
    /// Not useful for single tile changes.
    /// </summary>
    public void SetWorldConditions()
    {
        //Setup the world nodes
        worldNodes = new Node[LevelGenerator.current.levelSize, LevelGenerator.current.levelSize];
        //Fill the world nodes array with default values
        for (int x = 0; x < LevelGenerator.current.levelSize; x++)
        {
            for (int y = 0; y < LevelGenerator.current.levelSize; y++)
            {
                //Console.WriteLine($"Setting up {x}, {y}");
                Node newNode = new Node(x, y);
                newNode.SetBlocked(LevelGenerator.current.turfs[x, y].occupied);
                worldNodes[x, y] = newNode;
            }
        }
        Log.PrintDebug("Pathfinding system setup and ready.");
    }

    /// <summary>
    /// Runs a pathfinding requests for a debug dummy path
    /// </summary>
    public void RequestDebugPath()
    {
        PathfindingRequest request = new PathfindingRequest();
        request.start_x = 4;
        request.start_y = 10;
        request.end_x = 40;
        request.end_y = 40;
        CalculatePath(request);
    }

    /// <summary>
    /// Calculates the path.
    /// We can do it all in 1 go, this system is on a seperate thread so it doesn't matter too much if we take a while.
    /// This is probably a pretty ineffecient implementation but its on a seperate thread, gets the job done and isn't too critical for the project.
    /// </summary>
    /// <param name="request"></param>
    public void CalculatePath(PathfindingRequest request)
    {
        pathId++;
        //The list of nodes that can be searched
        //Everything in this list should have updated costs values before going in
        //since we don't reset and recaclulate every time so the old path values will
        //be carried over otherwise.
        List<Node> searchNodes = new List<Node>();
        //Cache the end node because its quicker than checking list every time
        Node targetNode = worldNodes[request.end_x, request.end_y];
        //Calculate initial nodes (The ones adjacent)
        SetupInitialConditions(ref searchNodes, request);
        //Cool lets just keep searching until we run out of things to search
        //If we have to search more than X nodes, just assume its impossible.
        int sanity = 20000;
        Stopwatch timer = new Stopwatch();
        timer.Start();
        while (sanity > 0 && searchNodes.Count > 0)
        {
            sanity --;
            //Might be a way with cachine to make this faster, this is quite slow :(
            //We could use a sorted list, binary search and insertion in order?#
            //Would be tons faster
            //(We do all this mini optimisations then have this as super slow!)
            int lowestIndex = 0;
            float lowestCost = searchNodes[lowestIndex].GetTotalCost();
            for (int i = 1; i < searchNodes.Count; i++)
            {
                float testCost = searchNodes[i].GetTotalCost();
                if (testCost < lowestCost)
                {
                    lowestCost = testCost;
                    lowestIndex = i;
                }
            }
            //We know the node to search now
            Node testingNode = searchNodes[lowestIndex];
            //Console.WriteLine($"Checking: {testingNode.x}, {testingNode.y} with cost: {testingNode.GetTotalCost()}");
            //Have we reached the end?
            if (testingNode == targetNode)
            {
                List<Node> quickestPath = testingNode.GetRecursiveNodes();
                List<Turf> pathTurfs = new List<Turf>();
                Log.PrintDebug($"Found path length: {quickestPath.Count}");
                foreach(Node node in quickestPath)
                {
                    Turf locatedBase = LevelGenerator.current.turfs[node.x, node.y];
                    pathTurfs.Add(locatedBase);
                }
                Path finalPath = new Path();
                finalPath.calculatedRoute = pathTurfs;
                request.CompletePath(finalPath);
                timer.Stop();
                Log.PrintDebug($"Found path in {timer.ElapsedMilliseconds}ms");
                return;
            }
            //Add surrounding nodes
            AddSurroundingNodes(ref searchNodes, testingNode, request);
            //Block it from being re-added
            testingNode.isChecked = true;
            //Thats all the checking we needed to do
            searchNodes.Remove(testingNode);
        }
        //No path found
        request.FailPath();
        Log.PrintDebug("No path was found");
        Log.PrintDebug($"Failed to find path in {timer.ElapsedMilliseconds}ms");
    }

    /// <summary>
    /// Adds the surround nodes to a source node, and sets their conditions.
    /// </summary>
    private void AddSurroundingNodes(ref List<Node> toCheckList, Node source, PathfindingRequest request)
    {
        //Get the adjacent nodes
        List<Node> adjacentNodes = GetAdjacentNodes(source);
        //Check each node individually
        //(secretly an embedded for loop which is a bit slow, but its only 4 long)
        for(int i = 0; i < adjacentNodes.Count; i++)
        {
            Node current = adjacentNodes[i];
            if (current.x == request.start_x && current.y == request.start_y)
            {
                continue;
            }
            //Ignore blocked nodes
            if (current.GetBlocked())
            {
                continue;
            }
            //In this case the node next has a parent node.
            //We need to check if by making source the parent node to current will make
            //a quicker path
            if (current.originNode != null && current.pathId == pathId)
            {
                //Our g cost will be the parent nodes gCost add 1
                float newGCost = current.originNode.gCost + 1;
                //Quicker to path through source
                if (newGCost < current.gCost)
                {
                    current.SetCosts(newGCost, current.hCost);
                    current.originNode = source;
                }
            }
            //In this case current is independant and has no parent
            //So we can add source as the parent instantly.
            else
            {
                //We come from source
                current.originNode = source;
                //We are 1 further from source from the start
                current.SetCosts(source.gCost + 1, QDistanceCalculation(current.x, current.y, request.end_x, request.end_y));
                //We are updated for this path
                current.pathId = pathId;
                //We need to check this
                if(!current.isChecked) toCheckList.Add(current);
            }
        }
    }

    /// <summary>
    /// Adds the initial nodes to check and updates them to what we need.
    /// </summary>
    /// <param name="nodeChecklist"></param>
    private void SetupInitialConditions(ref List<Node> nodeChecklist, PathfindingRequest request)
    {
        Node firstNode = worldNodes[request.start_x, request.start_y];
        //Setup the first node
        firstNode.SetCosts(0, QDistanceCalculation(request.start_x, request.start_y, request.end_x, request.end_y));
        firstNode.originNode = null;
        firstNode.pathId = pathId;
        firstNode.isChecked = true;
        List<Node> firstNodes = GetAdjacentNodes(firstNode);
        foreach (Node node in firstNodes)
        {
            //The node is being worked on this path
            node.pathId = pathId;
            //We connect to the main node simply
            node.originNode = firstNode;
            //Calculate the costs.
            node.SetCosts(1, QDistanceCalculation(node.x, node.y, firstNode.x, firstNode.y));
            //Add it
            nodeChecklist.Add(node);
        }
    }

    /// <summary>
    /// Calculating based on pythagorus is expensive,
    /// so we are just going to assume the distance = xChange + yChange.
    /// We don't calculate curves anyway, so this probably shouldn't affect us too much?
    /// </summary>
    private int QDistanceCalculation(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
    }

    /// <summary>
    /// Gets the adjacent nodes.
    /// Ignores diagonals since its easier that way (dont want paths going diagonally through walls).
    /// Amazing diagram:
    ///   # *
    ///   #* <--- It just went through a wall.
    ///   *##
    ///  *  #
    /// </summary>
    /// <returns>adjacent nodes</returns>
    public List<Node> GetAdjacentNodes(Node origin)
    {
        List<Node> nodes = new List<Node>();
        //Check that we aren't on the border of the world and add the adjacent node.
        //Checks for left, right, up, down.
        if (origin.x != LevelGenerator.current.levelSize - 1)
        {
            nodes.Add(worldNodes[origin.x + 1, origin.y]);
        }
        if (origin.x != 0)
        {
            nodes.Add(worldNodes[origin.x - 1, origin.y]);
        }
        if (origin.y != LevelGenerator.current.levelSize - 1)
        {
            nodes.Add(worldNodes[origin.x, origin.y + 1]);
        }
        if (origin.y != 0)
        {
            nodes.Add(worldNodes[origin.x, origin.y - 1]);
        }
        return nodes;
    }

    /// <summary>
    /// Calculates a path to an ending position
    /// Takes into account obstacles.
    /// Async method only returns when path is found.
    /// </summary>
    /*public async Path GetPath(Base startingPosition, Base endingPosition)
    {
        //lol
        return null;
    }*/

}
