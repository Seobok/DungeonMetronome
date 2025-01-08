using System;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// 방 하나를 관리하는 스크립트
    /// 각 방은 가로 11칸 세로 7칸을 가지고 있으며 이는 벽을 포함한 칸 수 이다.
    /// Room의 Position은 방의 좌측 하단이고 다음 방의 Position까지 x는 11, y 는 7 떨어져있다. 
    /// </summary>
    public class Room : MonoBehaviour
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Dungeon Dungeon { get; set; }
        public Room UpSideRoom => Y == Dungeon.DUNGEON_Y - 1 ? null : Dungeon.Rooms[X, Y + 1];
        public Room LeftSideRoom => X == 0 ? null : Dungeon.Rooms[X - 1, Y];
        public Room DownSideRoom => Y == 0 ? null : Dungeon.Rooms[X, Y - 1];
        public Room RightSideRoom => X == Dungeon.DUNGEON_X ? null : Dungeon.Rooms[X + 1, Y];
        
        
        public const int X_LENGTH = 11;
        public const int Y_LENGTH = 7;

        private Tile _tilePrefab;
        private Tile[,] _tiles;

        
        private void Awake()
        {
            _tiles = new Tile[X_LENGTH, Y_LENGTH];
            
            if (!_tilePrefab)
            {
                _tilePrefab = Resources.Load<Tile>("Prefabs/Map/Tile");
            }
            
            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    Tile go = Instantiate(_tilePrefab, transform);

                    _tiles[i, j] = go;
                    go.name = "Tile_" + i + "_" + j;
                    go.X = i;
                    go.Y = j;
                    go.Room = this;
                    go.transform.localPosition = new Vector3(i - X_LENGTH / 2, j - Y_LENGTH / 2, 0);
                }
            }
        }

        public Tile GetTile(int x, int y)
        {
            if(x < 0)
            {
                return LeftSideRoom ? LeftSideRoom.GetTile(X_LENGTH + x, y) : null;
            }
            if(y < 0)
            {
                return DownSideRoom ? DownSideRoom.GetTile(x, Y_LENGTH + y) : null;
            }
            if(x >= X_LENGTH)
            {
                return RightSideRoom ? RightSideRoom.GetTile(x - X_LENGTH, y) : null;
            }
            if(y >= Y_LENGTH)
            {
                return UpSideRoom ? UpSideRoom.GetTile(x, y - Y_LENGTH) : null;
            }

            return _tiles[x, y];
        }
    }
}
