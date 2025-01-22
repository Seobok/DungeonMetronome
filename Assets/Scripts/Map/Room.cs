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
        public int X { get; set; }

        /// <summary>
        /// Dungeon의 Room관리 배열에서의 Index Y
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 실제 Tile이 설치될 Room Position의 중앙
        /// </summary>
        public Vector2 CenterPos { get; set; }

        public Dungeon Dungeon { get; set; }

        public Room UpSideRoom => Y == Dungeon.DUNGEON_Y - 1 ? null : Dungeon.Rooms[X, Y + 1];

        public Room LeftSideRoom => X == 0 ? null : Dungeon.Rooms[X - 1, Y];

        public Room DownSideRoom => Y == 0 ? null : Dungeon.Rooms[X, Y - 1];

        public Room RightSideRoom => X == Dungeon.DUNGEON_X ? null : Dungeon.Rooms[X + 1, Y];
        

        public const int X_LENGTH = 11;
        public const int Y_LENGTH = 7;

        private Tile[,] _tiles;

        
        public Room()
        {
            _tiles = new Tile[X_LENGTH, Y_LENGTH];
        }

        public void SpawnTiles()
        {
            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    Tile tile = new Tile();

                    _tiles[i, j] = tile;
                    tile.X = i;
                    tile.Y = j;
                    tile.Room = this;
                    tile.Position = new Vector2(i - X_LENGTH / 2 + CenterPos.x, j - Y_LENGTH / 2 + CenterPos.y);
                }
            }
        }

        public Tile GetTile(int x, int y)
        {
            if(x < 0)
            {
                return LeftSideRoom?.GetTile(X_LENGTH + x, y);
            }
            if(y < 0)
            {
                return DownSideRoom?.GetTile(x, Y_LENGTH + y);
            }
            if(x >= X_LENGTH)
            {
                return RightSideRoom?.GetTile(x - X_LENGTH, y);
            }
            if(y >= Y_LENGTH)
            {
                return UpSideRoom?.GetTile(x, y - Y_LENGTH);
            }

            return _tiles[x, y];
        }
    }
}
