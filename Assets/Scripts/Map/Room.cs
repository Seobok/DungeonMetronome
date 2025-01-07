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
        public Tile[,] Tiles { get; private set; }
        
        
        public const int X_LENGTH = 11;
        public const int Y_LENGTH = 7;

        private Tile _tilePrefab;

        
        private void Awake()
        {
            Tiles = new Tile[X_LENGTH, Y_LENGTH];
            
            if (!_tilePrefab)
            {
                _tilePrefab = Resources.Load<Tile>("Prefabs/Map/Tile");
            }
            
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tile go = Instantiate(_tilePrefab, transform);

                    Tiles[i, j] = go;
                    go.name = "Tile_" + i + "_" + j;
                    go.X = i;
                    go.Y = j;
                    go.transform.localPosition = new Vector3(i - X_LENGTH / 2, j - Y_LENGTH / 2, 0);
                }
            }
        }
    }
}
