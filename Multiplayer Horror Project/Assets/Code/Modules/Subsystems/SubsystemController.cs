using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsystemController : MonoBehaviour
{

    public static SubsystemController singleton;

    public List<Subsystem> subsystems = new List<Subsystem>();

    private void Start()
    {

        //Make sure there is only 1
        if (singleton)
        {
            LogHandler.Log("Warning, 2 instances of SubsystemController created");
            Destroy(this);
            return;
        }

        singleton = this;

        IEnumerable<Type> subsystemTypes = Utility.SubtypesOf(typeof(Subsystem));
        foreach (Type systemType in subsystemTypes)
        {
            Subsystem SS = (Subsystem)Activator.CreateInstance(systemType);
            SS.Initialize();
            subsystems.Add(SS);
        }

    }

}
