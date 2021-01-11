using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PREGAME,
    GAME,
}

/// <summary>
/// Basically what controls the flow of the game
/// </summary>
public class Ticker : Subsystem
{

    public Gamemode gamemode;
    public GameState gameState = GameState.PREGAME;

    public override void Initialize()
    {
        fire_delay = 5000;
        base.Initialize();
        gamemode = new ContainmentBreach();
    }

    public override void Fire()
    {

        LogHandler.Log("E");

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        //Ticking
        switch (gameState)
        {
            case GameState.PREGAME:
                if (PhotonNetwork.PlayerList.Length > 1)
                {
                    LogHandler.Log("Starting game...");
                    gamemode.OnStart();
                    gameState = GameState.GAME;
                }
                else
                {
                    LogHandler.Log("Waiting for players...");
                }
                return;
        }
    }

}
