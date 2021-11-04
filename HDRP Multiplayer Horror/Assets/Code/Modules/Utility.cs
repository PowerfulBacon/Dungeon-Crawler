using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public static class Utility
{
    public static IEnumerable<Type> SubtypesOf(Type type)
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(type));
    }
}
