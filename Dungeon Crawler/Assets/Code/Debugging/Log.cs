using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Log
{

    public static void Print(string message, bool debugMode = true)
    {
        if (Master.DEBUG_MODE && debugMode)
        {
            Debug.Log(message);
        }
    }

}
