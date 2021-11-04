using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers
{

    public static T Pick<T>(T[] objects)
    {
        return objects[Random.Range(0, objects.GetLength(0))];
    }

    public static T Pick<T>(List<T> objects)
    {
        return objects[Random.Range(0, objects.Count)];
    }

}