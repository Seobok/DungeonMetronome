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
        private List<Enemy.Enemy> _enemies = new List<Enemy.Enemy>(); // 소환된 모든 Enemy를 관리
        
        
        /// <summary>
        /// 나이트 캐릭터를 해당 위치에 소환
        /// </summary>
        public Knight SpawnKnight(int x, int y)
        {
            Knight = new Knight(_dungeon, this);
            Knight.Transform.position = new Vector3(x, y, -1);
            
            return Knight;
        }

        /// <summary>
        /// (X, Y) 위치에 type Enemy를 스폰하는 함수
        /// </summary>
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
            _dungeon.Tiles[x + Dungeon.DUNGEON_X * Room.X_LENGTH / 2][y + Dungeon.DUNGEON_Y * Room.Y_LENGTH / 2].Unit = enemy;
            
            _enemies.Add(enemy);
            
            return enemy;
        }

        public void RemoveEnemy(Enemy.Enemy enemy)
        {
            _enemies.Remove(enemy);
        }
        
        public void ActOnAllEnemies()
        {
            foreach (Enemy.Enemy enemy in _enemies)
            {
                enemy.Act();
            }
        }
    }
}