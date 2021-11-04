using System.Collections;
using System.Collections.Generic;
using Dungeon_Crawler.Assets.Code.UserInterface.Chat;
using UnityEngine;

public static class Log
{

    public static void PrintError(string message, bool debugMode = true)
    {
        if (Master.DEBUG_MODE && debugMode)
        {
            Debug.LogError($"<color=red><b>{message}</b></color>");
            Chat.ToChat($"<style=danger>ERROR:</b> {message}</style>");
        }
    }

    public static void PrintDebug(string message)
    {
        if (Master.DEBUG_MODE)
        {
            Debug.Log(message);
            Chat.ToChat($"<style=debug>DEBUG: {message}</style>");
        }
    }

    public static void ServerMessage(string message, bool debugMode = true)
    {
        if (Master.DEBUG_MODE && debugMode)
        {
            Debug.Log(message);
            Chat.ToChat($"<style=servermessage>{message}</style>");
        }
    }

    public static void Print(string message, bool debugMode = true)
    {
        if (Master.DEBUG_MODE && debugMode)
        {
            Debug.Log(message);
            Chat.ToChat(message);
        }
    }

}
