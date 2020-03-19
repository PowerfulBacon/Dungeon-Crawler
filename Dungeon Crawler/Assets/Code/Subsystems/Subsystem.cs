using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class Subsystem
{

    public static int SERVER_TICK_RATE = 20;

    public Dictionary<string, object> subsystemQuery = new Dictionary<string, object>();

    public string subsystemName;
    public float processingTime;

    public Subsystem(string name = "")
    {
        subsystemName = name;
        Initialise();
    }

    public void Request(string requestName, object requestData = null)
    {
        subsystemQuery.Add(requestName, requestData);
    }

    protected bool CheckRequest(string request)
    {
        return subsystemQuery.ContainsKey(request);
    }

    protected object GetRequestData(string request, bool deleteAfter = false)
    {
        object data = subsystemQuery[request];
        if (deleteAfter)
            subsystemQuery.Remove(request);
        return data;
    }

    public virtual void Initialise()
    {
        
    }

    public IEnumerator UpdateThreadMaster()
    {
        float timeBetweenUpdates = 1.0f / SERVER_TICK_RATE;
        while (true)
        {
            long startTime = DateTime.Now.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
            Update();
            long endTime = DateTime.Now.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
            processingTime = (endTime - startTime) / 1000000.0f;
            if (processingTime >= timeBetweenUpdates)
                Log.Print("Warning! Subsystem [" + subsystemName + "] took " + processingTime + " seconds to update");
            float timeToWait = Mathf.Clamp(timeBetweenUpdates - processingTime, 0, timeBetweenUpdates);
            yield return new WaitForSeconds(timeToWait);
        }
    }

    protected virtual void Update()
    {

    }

}
