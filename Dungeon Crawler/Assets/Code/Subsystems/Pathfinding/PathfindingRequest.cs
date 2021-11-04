using System;
using System.Collections.Generic;
using System.Text;
using Dungeon_Crawler;

public class PathfindingRequest
{

    public delegate void OnPathfindingCompletion(Path foundPath);
    public OnPathfindingCompletion onPathfindingCompletion;

    public delegate void OnPathfindingFailure();
    public OnPathfindingFailure onPathfindingFailure;

    public int start_x;
    public int start_y;
    public int end_x;
    public int end_y;

    public void CompletePath(Path foundPath)
    {
        if(onPathfindingCompletion != null) onPathfindingCompletion(foundPath);
    }

    public void FailPath()
    {
        if(onPathfindingFailure != null) onPathfindingFailure();
    }

}

