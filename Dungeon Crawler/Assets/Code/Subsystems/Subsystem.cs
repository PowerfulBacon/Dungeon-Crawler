using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Subsystem
{

    public Dictionary<string, object> subsystemQuery = new Dictionary<string, object>();

    public Subsystem()
    {
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
        while (true)
        {
            Update();
            yield return null;
        }
    }

    protected virtual void Update()
    {

    }

}
