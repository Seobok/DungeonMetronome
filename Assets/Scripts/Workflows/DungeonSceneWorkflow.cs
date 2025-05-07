using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Map;
using Unit;
using Unit.Enemy;
using Unit.Player;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Workflows
{
    public class DungeonSceneWorkflow : MonoBehaviour
    {
        private Dungeon _dungeon;
        private UnitManager _unitManager;


        [Inject]
        public void Construct(Dungeon dungeon, UnitManager unitManager)
        {
            _dungeon = dungeon;
            _unitManager = unitManager;
        }
        
        private void Start()
        {
            _dungeon.ActivateDungeon(7);

            Knight knight = _unitManager.SpawnKnight(0, 0);
            knight.PlayerController.NextTurn += NextTurn;

            // 적 15마리 스폰
            for (int i = 0; i < 15; i++)
            {
                Coord spawnPosition = _dungeon.GetRandomEmptyTileFromRooms(100);
                
                if (spawnPosition != Coord.Zero) // 적절한 위치를 찾은 경우
                {
                    int random = Random.Range(0, 2);
                    switch (random)
                    {
                        case 0:
                            _unitManager.SpawnEnemy(typeof(Bat), spawnPosition.X, spawnPosition.Y);
                            break;
                        case 1:
                            _unitManager.SpawnEnemy(typeof(Slime), spawnPosition.X, spawnPosition.Y);
                            break;
                    }
                    
                }
                else
                {
                    Debug.LogWarning("더 이상 적을 스폰할 수 있는 빈 공간이 없습니다.");
                    break;
                }
            }
        }

        public void NextTurn()
        {
            _unitManager.ActOnAllEnemies();
        }
    }
}
