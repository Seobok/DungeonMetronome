using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    static int[] dirX = new int[4] { 0, 1, 0, -1 };
    static int[] dirY = new int[4] { 1, 0, -1, 0 };

    public static Stack<Tile> FindPath(Tile start, Tile end)
    {
        bool pathSuccess = false;
        Stack<Tile> path= new Stack<Tile>();

        if(end.onTileUnit == null)
        {
            List<Tile> openSet = new List<Tile>();
            HashSet<Tile> closedSet = new HashSet<Tile>();

            start.gCost = 0;
            start.hCost = GetDistance(start, end);

            openSet.Add(start);

            while(openSet.Count > 0)
            {
                Tile currentTile = openSet[0];

                for(int i = 1; i< openSet.Count; i++)
                {
                    //fCost가 가장 낮은 타일을 현재 타일로 지정
                    if ((openSet[i].fCost < currentTile.fCost) || ((openSet[i].fCost == currentTile.fCost) && (openSet[i].hCost < currentTile.hCost)))
                    {
                        currentTile = openSet[i];
                    }
                }

                openSet.Remove(currentTile);
                closedSet.Add(currentTile);
                Debug.Log($"currentTile : {currentTile.x} {currentTile.y}");

                if (currentTile == end)
                {
                    if(!pathSuccess)
                    {
                        pathSuccess = true;
                        Debug.Log($"Complete PathFind");
                    }
                    break;
                }

                for(int i=0;i<4;i++)
                {
                    var neighbor = currentTile.parentRoom.GetTile(currentTile.x + dirX[i], currentTile.y + dirY[i]);
                    if (neighbor == null || neighbor.onTileUnit != null) continue;
                    if (closedSet.Contains(neighbor)) continue;
                    if(openSet.Contains(neighbor) && currentTile.gCost + GetDistance(currentTile, neighbor) >= neighbor.gCost) continue;

                    neighbor.gCost = currentTile.gCost + GetDistance(currentTile, neighbor);
                    neighbor.hCost = GetDistance(neighbor, end);
                    neighbor.astarParent = currentTile;

                    Debug.Log($"AddOpenSet : {neighbor.x} {neighbor.y}");

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
            Tile pathTile = end;
            while(pathTile != start)
            {
                path.Push(pathTile);
                Debug.Log(pathTile.x + " " + pathTile.y);
                pathTile = pathTile.astarParent;
            }
        }
        return path;
    }

    public static int GetDistance(Tile start, Tile end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }
}
