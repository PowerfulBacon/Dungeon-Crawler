using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;


public class Observer : Mob
{

    public List<Atom> trackableObjects;
    public Atom trackedAtom = null;

    public override void ClientLife()
    {
        base.ClientLife();

        trackableObjects = GetTrackableEntities();

        //Handle key presses
        int direction = 0;
        if (client.keyMap.GetKeyState("LeftMouse"))
            direction = 1;

        if (client.keyMap.GetKeyState("RightMouse"))
            direction = -1;

        if (direction != 0)
        {
            List<Atom> trackable = GetTrackableEntities();
            if (trackable.Count != 0)
            {
                int currentIndex = trackable.IndexOf(trackedAtom);
                int nextIndex = (currentIndex + direction) % trackable.Count;
                trackedAtom = trackable[nextIndex];
            }
        }

        //Start tracking
        if (trackedAtom != null)
        {
            if (transform.parent != trackedAtom)
            {
                transform.SetParent(trackedAtom.transform);
                transform.localPosition = UnityEngine.Vector3.zero;
                transform.localRotation = UnityEngine.Quaternion.identity;
            }
        }
    }

    public virtual List<Atom> GetTrackableEntities()
    {
        List<Atom> trackableAtoms = new List<Atom>();
        trackableAtoms.AddRange(FindObjectsOfType<Human>());
        return trackableAtoms;
    }

}
