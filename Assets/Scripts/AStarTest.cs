using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
    public static AStarTest instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public Tile start;
    public Tile end;

    public void TestPathFinding()
    {
        if(start != null && end != null)
        {
            AStar.FindPath(start, end);
        }
    }
}
