using System;
using System.Collections.Generic;
using Unit.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class Dungeon
    {
        public Dungeon()
        {
            //tile 프리팹 받아오기
            _tilePrefab = Resources.Load<GameObject>("Prefabs/Map/Tile");
            
            //rooms 배열 초기화
            for (int i = 0; i < DUNGEON_X; i++)
            {
                _rooms[i] = new Room[DUNGEON_Y];
            }

            //Tiles 배열 초기화
            for (int i = 0; i < DUNGEON_X * Room.X_LENGTH; i++)
            {
                Tiles[i] = new Tile[DUNGEON_Y * Room.Y_LENGTH];
            }
            
            InitializeRooms();
        }
        
        
        public Tile[][] Tiles { get; private set; } = new Tile[DUNGEON_X * Room.X_LENGTH][];
        public Dictionary<Coord, GameObject> TileObjects { get; set; } = new Dictionary<Coord, GameObject>(1000);


        public const int DUNGEON_X = 15;
        public const int DUNGEON_Y = 15;
        private const int MAX_ROOM_COUNT = 150;

        private Room _startRoom;
        private List<Room> _roomPool;
        private readonly Coord[] _directions = new Coord[] { new Coord(0, 1), new Coord(1, 0), new Coord(0, -1), new Coord(0, -1) };
        private readonly Room[][] _rooms = new Room[DUNGEON_X][];
        private readonly GameObject _tilePrefab;
        
        
        /// <summary>
        /// 방을 생성해서 풀링
        /// </summary>
        private void InitializeRooms()
        {
            _roomPool = new List<Room>(MAX_ROOM_COUNT);

            //특정 갯수의 방이 만들어 질때까지 반복
            for (int i = 0; i < MAX_ROOM_COUNT; i++)
            {
                //방 생성 가능
                Room newRoom = new Room(this);
                if (i == 0)
                {
                    _startRoom = newRoom;
                }

                _roomPool.Add(newRoom);
            }
        }

        /// <summary>
        /// 풀링 된 방을 정해진 갯수에 맞춰 재조정
        /// </summary>
        /// <param name="roomCount"> 처리해야하는 방 갯수 </param>
        /// <exception cref="ArgumentException"> 정상적이지 않은 방 갯수가 입력됨 </exception>
        public void ActivateDungeon(int roomCount)
        {
            if (roomCount > MAX_ROOM_COUNT || roomCount < 1)
                throw new ArgumentException();
            
            //첫 방은 가운데 고정
            Room startRoom = _startRoom;
            startRoom.XInDungeon = DUNGEON_X / 2;
            startRoom.YInDungeon = DUNGEON_Y / 2;
            _rooms[startRoom.XInDungeon][startRoom.YInDungeon] = startRoom;
            startRoom.SpawnTiles();
            
            int setCount = 1;   //위치가 설정된 방의 갯수
            while (setCount < roomCount)
            {
                Room room = _roomPool[Random.Range(0, setCount)];
                Coord direction = _directions[Random.Range(0, _directions.Length)];
                
                int nextRoomX = room.XInDungeon + (int)direction.X;
                int nextRoomY = room.YInDungeon + (int)direction.Y;

                //Rooms의 배열 범위에 들어가는지 확인
                if (nextRoomX >= 0 && nextRoomY >= 0 && nextRoomX < DUNGEON_X && nextRoomY < DUNGEON_Y)
                {
                    if (_rooms[nextRoomX][nextRoomY] == null)
                    {
                        _roomPool[setCount].XInDungeon = nextRoomX;
                        _roomPool[setCount].YInDungeon = nextRoomY;
                        _rooms[nextRoomX][nextRoomY] = _roomPool[setCount];

                        _roomPool[setCount].CenterPos = room.CenterPos + new Coord(direction.X * Room.X_LENGTH, direction.Y * Room.Y_LENGTH);
                        _roomPool[setCount].SpawnTiles();
                        
                        setCount++;
                    }
                }
            }
        }

        /// <summary>
        /// 다시 방 풀로 되돌리기
        /// </summary>
        public void DeactivateDungeon()
        {
            //Rooms 초기화
        }

        public void RegisterTile(Tile tile)
        {
            GameObject tileGo = GameObject.Instantiate(_tilePrefab);
            tileGo.transform.position = new Vector3(tile.Coord.X, tile.Coord.Y, 0);

            Tiles[tile.Coord.X + DUNGEON_X * Room.X_LENGTH / 2][tile.Coord.Y + DUNGEON_Y * Room.Y_LENGTH / 2] = tile;
            TileObjects.Add(tile.Coord, tileGo);
        }

        public void GetTile(int x, int y, out Tile tile)
        {
            tile = default;

            x += DUNGEON_X * Room.X_LENGTH / 2;
            y += DUNGEON_Y * Room.Y_LENGTH / 2;

            if (x < 0 || y < 0 || x >= DUNGEON_X * Room.X_LENGTH || y >= DUNGEON_Y * Room.Y_LENGTH)
                return;

            tile = Tiles[x][y];
        }
    }
}