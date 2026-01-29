using System;
using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine;

namespace Utility
{
    public struct PathFindCost : IEquatable<PathFindCost>
    {
        public int FCost => GCost + HCost;
        public int GCost;
        public int HCost;
        public Tile AStarParent;

        public bool Equals(PathFindCost other)
        {
            return GCost == other.GCost && HCost == other.HCost;
        }

        public override bool Equals(object obj)
        {
            return obj is PathFindCost other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GCost, HCost);
        }
    }
    public static class PathFind
    {
        private static readonly Vector2[] Dir = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        
        public static Stack<Tile> FindPath(this Dungeon dungeon, Tile start, Tile end)
        {
            bool pathSuccess = false;
            Stack<Tile> path= new Stack<Tile>();

            if (!TileRules.IsBlockedForEnemy(end) || end.Player != null)
            {
                List<Tile> openSet = new List<Tile>();
                Dictionary<Tile,PathFindCost> tileCosts = new Dictionary<Tile,PathFindCost>();
                HashSet<Tile> closedSet = new HashSet<Tile>();

                PathFindCost startCost = new PathFindCost()
                {
                    GCost = 0,
                    HCost = GetDistance(start, end),
                };

                openSet.Add(start);
                tileCosts.Add(start,startCost);

                while(openSet.Count > 0)
                {
                    Tile currentTile = openSet[0];
                    PathFindCost currentCost = tileCosts[openSet[0]];

                    //currentTile보다 적합한 타일이 있는지 찾아보기
                    for(int i = 1; i< openSet.Count; i++)
                    {
                        if ((tileCosts[openSet[i]].FCost < currentCost.FCost) || ((tileCosts[openSet[i]].FCost == currentCost.FCost) && (tileCosts[openSet[i]].HCost < currentCost.HCost)))
                        {
                            currentTile = openSet[i];
                            currentCost = tileCosts[openSet[i]];
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
                        Tile neighbor = dungeon.GetTile(currentTile.Coord.X + (int)Dir[i].x, currentTile.Coord.Y + (int)Dir[i].y);
                        // 이웃에 대한 검사
                        // 1. 타일이 존재하지 않거나, 타일위에 다른 대상이 있어 지나갈수 없는 경우 갈 수 없음
                        // 2. 이미 한번 지난 타일은 갈 수 없음
                        // 3. 한번 거리를 측정한 적이 있는데 해당 측정한 경우가 더 짧은 경우
                        if (neighbor != end && TileRules.IsBlockedForEnemy(neighbor)) continue;
                        if (closedSet.Contains(neighbor)) continue;
                        if (openSet.Contains(neighbor) && tileCosts[currentTile].GCost + GetDistance(currentTile, neighbor) >= tileCosts[neighbor].GCost) continue;

                        tileCosts[neighbor] = new PathFindCost()
                        {
                            GCost = tileCosts[currentTile].GCost + GetDistance(currentTile, neighbor),
                            HCost = GetDistance(neighbor, end),
                            AStarParent = currentTile,
                        };

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
                    pathTile = tileCosts[pathTile].AStarParent;
                }
            }
            return path;
        }

        /// <summary>
        /// 일정 범위의 타일을 받아오는 함수
        /// </summary>
        /// <param name="dungeon"></param>
        /// <param name="currentTile"> 해당 타일을 기준으로 거리를 측정 </param>
        /// <param name="distance"> 범위 </param>
        /// <returns></returns>
        public static List<Tile> GetTilesInDistance(this Dungeon dungeon, Tile currentTile, int distance)
        {
            List<Tile> tiles = new List<Tile>();
            
            Queue<Tile> queue = new Queue<Tile>();
            queue.Enqueue(currentTile);
            tiles.Add(currentTile);
            
            while (queue.Count > 0)
            {
                Tile curTile = queue.Dequeue();
                
                for (int i = 0; i < 4; i++)
                {
                    Tile nextTile = dungeon.GetTile(curTile.Coord.X + (int)Dir[i].x, curTile.Coord.Y + (int)Dir[i].y);
                    if (tiles.Contains(nextTile)) continue;
                    if (TileRules.IsBlockedForEnemy(nextTile) && nextTile.Player == null) continue;
                    if (GetDistance(currentTile,nextTile) > distance) continue;
                    
                    tiles.Add(nextTile);
                    queue.Enqueue(nextTile);
                }
            }

            return tiles;
        }
        
        public static int GetDistance(Tile start, Tile end)
        {
            return Mathf.Abs(start.Coord.X - end.Coord.X) + Mathf.Abs(start.Coord.Y - end.Coord.Y);
        }
    }
}
