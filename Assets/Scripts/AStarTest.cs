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
            var list = AStar.FindPath(start, end);
            foreach(var item in list)
            {
                item.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
}
