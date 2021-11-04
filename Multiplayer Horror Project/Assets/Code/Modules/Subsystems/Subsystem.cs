using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class Subsystem
{

    //The name of the subsystem
    public string name = "undefined";

    public int fire_delay = 100;  //Time between firing in milliseconds

    public virtual void Initialize()
    {
        Thread systemThread = new Thread(new ThreadStart(Update));
        systemThread.Start();
    }

    private void Update()
    {
        Stopwatch timer = Stopwatch.StartNew();
        try
        {
            Fire();
        }
        catch (Exception e)
        {
            LogHandler.Log(e.Message);
        }
        timer.Stop();
        Thread.Sleep((int)Math.Max(0, fire_delay - timer.ElapsedMilliseconds));
    }

    public virtual void Fire()
    { }

}
