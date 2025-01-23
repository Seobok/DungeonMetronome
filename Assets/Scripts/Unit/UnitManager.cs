using System;
using System.Collections.Generic;
using Map;
using Unit.Enemy;
using Unit.Player;
using UnityEngine;

namespace Unit
{
    public class UnitManager
    {
        public UnitManager(Dungeon dungeon)
        {
            Dungeon = dungeon;

            for (int i = 0; i < Dungeon.DUNGEON_X * Room.X_LENGTH; i++)
            {
                Enemies[i] = new Enemy.Enemy[Dungeon.DUNGEON_Y * Room.Y_LENGTH];
            }

            _batPrefab = Resources.Load<GameObject>("Prefabs/Enemy/Bat");
        }
        
        
        public Dungeon Dungeon { get; private set; }
        public GameObject KnightGameObject { get; private set; }

        public Dictionary<Enemy.Enemy, GameObject> EnemyObjects { get; private set; } =
            new Dictionary<Enemy.Enemy, GameObject>();
        public Enemy.Enemy[][] Enemies { get; set; } = new Enemy.Enemy[Dungeon.DUNGEON_X * Room.X_LENGTH][];
        
        public Knight Knight { get; set; }
        
        
        private readonly GameObject _batPrefab;
        
        
        /// <summary>
        /// 나이트 캐릭터를 해당 위치에 소환
        /// </summary>
        public Knight SpawnKnight(int x, int y)
        {
            GameObject knightPrefab = Resources.Load<GameObject>("Prefabs/Character/Knight");
            KnightGameObject = GameObject.Instantiate(knightPrefab);
            KnightGameObject.transform.position = new Vector2(x, y);
            
            Knight = new Knight
            {
                Manager = this,
            };
            
            return Knight;
        }

        public Enemy.Enemy SpawnEnemy(Type type, int x, int y)
        {
            Enemy.Enemy enemy = null;
            
            if (type == typeof(Bat))
            {
                enemy = new Bat()
                {
                    Position = new Coord(x, y),
                    Manager = this,
                };
                
                GameObject bat = GameObject.Instantiate(_batPrefab);
                bat.transform.position = new Vector2(x, y);
                Enemies[x][y] = enemy;
                EnemyObjects.Add(enemy, bat);
            }

            return enemy;
        }
    }
}