using System;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// 방 하나를 관리하는 스크립트
    /// 각 방은 가로 11칸 세로 7칸을 가지고 있으며 이는 벽을 포함한 칸 수 이다.
    /// Room의 Position은 방의 좌측 하단이고 다음 방의 Position까지 x는 11, y 는 7 떨어져있다. 
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Dungeon의 Room관리 배열에서의 Index X
        /// </summary>
        public int XInDungeon { get; set; }

        /// <summary>
        /// Dungeon의 Room관리 배열에서의 Index Y
        /// </summary>
        public int YInDungeon { get; set; }

        /// <summary>
        /// 실제 Tile이 설치될 Room Position의 중앙
        /// </summary>
        public Coord CenterPos { get; set; }


        public const int X_LENGTH = 11;
        public const int Y_LENGTH = 7;
        
        private readonly Dungeon _dungeon;
        private bool _isSpawnedTile = false;

        
        public Room(Dungeon dungeon)
        {
            _dungeon = dungeon;
        }

        public void SpawnTiles()
        {
            if (_isSpawnedTile) return;
            
            for (int i = -(X_LENGTH / 2); i <= X_LENGTH / 2; i++)
            {
                for (int j = -(Y_LENGTH / 2); j <= Y_LENGTH / 2; j++) 
                {
                    Tile tile = new Tile()
                    {
                        Coord = new Coord()
                        {
                            X = CenterPos.X + i,
                            Y = CenterPos.Y + j,
                        },
                        Status = StatusFlag.Empty,
                    };
                    _dungeon.RegisterTile(tile);
                }
            }
            
            _isSpawnedTile = true;
        }
    }
}
