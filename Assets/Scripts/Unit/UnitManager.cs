using System;
using System.Collections.Generic;
using Map;
using Unit.Enemy;
using Unit.Player;
using UnityEngine;
using VContainer;

namespace Unit
{
    public class UnitManager
    {
        public Knight Knight { get; private set; }


        [Inject] private Dungeon _dungeon;
        // Dictionary<Coord, UnitBase>
        
        
        /// <summary>
        /// 나이트 캐릭터를 해당 위치에 소환
        /// </summary>
        public Knight SpawnKnight(int x, int y)
        {
            Knight = new Knight(_dungeon, this);
            Knight.Transform.position = new Vector3(x, y, -1);
            
            return Knight;
        }

        public Enemy.Enemy SpawnEnemy(Type type, int x, int y)
        {
            Enemy.Enemy enemy;
            
            if (type == typeof(Bat))
            {
                enemy = new Bat(_dungeon, this)
                {
                    Position = new Coord(x, y),
                };
            }
            else
            {
                throw new Exception($"[{nameof(SpawnEnemy)}] Invalid type");
            }

            enemy.Transform.position = new Vector3(x, y, -1);
            _dungeon.Tiles[x][y].Unit = enemy;
            return enemy;
        }
    }
}