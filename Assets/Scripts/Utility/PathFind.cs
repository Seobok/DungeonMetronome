using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine;

namespace Utility
{
    public class PathFind
    {
        private static readonly Vector2[] Dir = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        
        public static Stack<Tile> FindPath(Tile start, Tile end)
        {
            bool pathSuccess = false;
            Stack<Tile> path= new Stack<Tile>();

            if(!end.OnTileUnit)
            {
                List<Tile> openSet = new List<Tile>();
                HashSet<Tile> closedSet = new HashSet<Tile>();

                start.GCost = 0;
                start.HCost = GetDistance(start, end);

                openSet.Add(start);

                while(openSet.Count > 0)
                {
                    Tile currentTile = openSet[0];

                    //currentTile보다 적합한 타일이 있는지 찾아보기
                    for(int i = 1; i< openSet.Count; i++)
                    {
                        if ((openSet[i].FCost < currentTile.FCost) || ((openSet[i].FCost == currentTile.FCost) && (openSet[i].HCost < currentTile.HCost)))
                        {
                            currentTile = openSet[i];
                        }
                    }

                    openSet.Remove(currentTile);
                    closedSet.Add(currentTile);

                    if (currentTile == end)
                    {
                        pathSuccess = true;
                        break;
                    }

                    // 선정된 타일에 대해 4방향의 타일을 검사
                    for(int i=0;i<4;i++)
                    {
                        Tile neighbor = currentTile.Room.GetTile(currentTile.X + (int)Dir[i].x, currentTile.Y + (int)Dir[i].y);
                        // 이웃에 대한 검사
                        // 1. 타일이 존재하지 않거나, 타일위에 다른 대상이 있어 지나갈수 없는 경우 갈 수 없음
                        // 2. 이미 한번 지난 타일은 갈 수 없음
                        // 3. 한번 거리를 측정한 적이 있는데 해당 측정한 경우가 더 짧은 경우
                        if (!neighbor || neighbor.OnTileUnit) continue;
                        if (closedSet.Contains(neighbor)) continue;
                        if (openSet.Contains(neighbor) && currentTile.GCost + GetDistance(currentTile, neighbor) >= neighbor.GCost) continue;

                        neighbor.GCost = currentTile.GCost + GetDistance(currentTile, neighbor);
                        neighbor.HCost = GetDistance(neighbor, end);
                        neighbor.AStarParent = currentTile;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }

                //해당 타일까지 갈 수 있는 길이 없는 경우
                if(!pathSuccess)
                {
                    return null;
                }
                
                Tile pathTile = end;
                while(pathTile != start)
                {
                    path.Push(pathTile);
                    pathTile = pathTile.AStarParent;
                }
            }
            return path;
        }

        /// <summary>
        /// 일정 범위의 타일을 받아오는 함수
        /// </summary>
        /// <param name="currentTile"> 해당 타일을 기준으로 거리를 측정 </param>
        /// <param name="distance"> 범위 </param>
        /// <param name="isThroughUnit"> 만약 false라면 아이템이나 몬스터등 유닛 너머를 볼 수 없음 </param>
        /// <returns></returns>
        public static List<Tile> GetTilesInDistance(Tile currentTile, int distance, bool isThroughUnit = false)
        {
            Queue<Tile> openSet = new Queue<Tile>();
            HashSet<Tile> closedSet = new HashSet<Tile>();
            
            currentTile.GCost = 0;
            
            openSet.Enqueue(currentTile);
            
            while (openSet.Count > 0)
            {
                Tile visitTile = openSet.Dequeue();
                
                if(!closedSet.Add(visitTile))
                    continue;

                if(visitTile.GCost >= distance) continue;

                for (int i = 0; i < 4; i++)
                {
                    Tile nextTile = visitTile.Room.GetTile(visitTile.X + (int)Dir[i].x, visitTile.Y + (int)Dir[i].y);
                    if(!nextTile) continue;
                    if (!isThroughUnit && nextTile.OnTileUnit) continue;
                    if(closedSet.Contains(nextTile)) continue;
                    
                    nextTile.GCost = visitTile.GCost + GetDistance(nextTile, visitTile);
                    openSet.Enqueue(nextTile);
                }
            }

            return closedSet.ToList();
        }
        
        private static int GetDistance(Tile start, Tile end)
        {
            return Mathf.Abs(start.X - end.X) + Mathf.Abs(start.Y - end.Y);
        }
    }
}
