
using System.Collections.Generic;

public enum Action
{
    //Move towards a coordinate point
    MoveToCoords,
    //Move towards a target
    MoveTowards,
}

public struct AiAction
{

    public Action actionId { get; set; }

    public Dictionary<string, object> parameters { get; set; }

}
