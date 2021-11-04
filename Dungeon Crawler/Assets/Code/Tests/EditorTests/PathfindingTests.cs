using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PathfindingTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestValidPath()
    {
        Pathfinding pathfinding = new Pathfinding("pathfinding");
        Assert.IsTrue(pathfinding.RunPathfindingTest(true));
    }

    // A Test behaves as an ordinary method
    [Test]
    public void TestInvalidPath()
    {
        Pathfinding pathfinding = new Pathfinding("pathfinding");
        Assert.IsFalse(pathfinding.RunPathfindingTest(false));
    }

}
