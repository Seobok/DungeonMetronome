using System;
using System.Collections.Generic;
using Unit.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public enum DungeonGenerationMode
    {
        RandomExpand,
        TutorialPreset
    }

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
        public void ActivateDungeon(int roomCount, DungeonGenerationMode mode)
        {
            switch (mode)
            {
                case DungeonGenerationMode.RandomExpand:
                    ActivateRandomDungeon(roomCount);
                    break;
                case DungeonGenerationMode.TutorialPreset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown dungeon generation mode.");
            }
        }

        private void ActivateRandomDungeon(int roomCount)
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

        public void GenerateTutorialLayout(TutorialDungeonLayout layout)
        {
            if (layout == null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            if (layout.rooms == null || layout.rooms.Count == 0)
            {
                Debug.LogWarning("Tutorial layout has no rooms to generate.");
                return;
            }

            Coord anchorCoord = layout.rooms[0].ToCoord();
            for (int i = 0; i < layout.rooms.Count; i++)
            {
                Coord roomCoord = layout.rooms[i].ToCoord();
                if (roomCoord.X < 0 || roomCoord.Y < 0 || roomCoord.X >= DUNGEON_X || roomCoord.Y >= DUNGEON_Y)
                {
                    Debug.LogWarning($"Tutorial room coord out of bounds: ({roomCoord.X}, {roomCoord.Y})");
                    continue;
                }

                if (i >= _roomPool.Count)
                {
                    Debug.LogWarning("Tutorial layout exceeds pooled room count.");
                    break;
                }

                Room room = _roomPool[i];
                room.XInDungeon = roomCoord.X;
                room.YInDungeon = roomCoord.Y;
                _rooms[roomCoord.X][roomCoord.Y] = room;

                int offsetX = roomCoord.X - anchorCoord.X;
                int offsetY = roomCoord.Y - anchorCoord.Y;
                room.CenterPos = new Coord(offsetX * Room.X_LENGTH, offsetY * Room.Y_LENGTH);
                room.SpawnTiles();
            }

        }

        /// <summary>
        /// 다시 방 풀로 되돌리기
        /// </summary>
        public void DeactivateDungeon()
        {
            //Rooms 초기화
        }
        
        /// <summary>
        /// 활성화된 방 중 랜덤으로 선택
        /// </summary>
        /// <returns>랜덤 Room 객체</returns>
        private Room GetRandomActiveRoom()
        {
            // 활성화된 방들만 필터링
            List<Room> activeRooms = new List<Room>();
            foreach (var roomRow in _rooms)
            {
                foreach (var room in roomRow)
                {
                    if (room != null) activeRooms.Add(room); // 비어 있지 않은 방만 추가
                }
            }

            if (activeRooms.Count == 0) return null; // 활성화된 방이 없으면 null 반환
            return activeRooms[Random.Range(0, activeRooms.Count)]; // 랜덤 방 반환
        }

        public void RegisterTile(Tile tile)
        {
            GameObject tileGo = GameObject.Instantiate(_tilePrefab);
            tileGo.transform.position = new Vector3(tile.Coord.X, tile.Coord.Y, 0);

            Tiles[tile.Coord.X + DUNGEON_X * Room.X_LENGTH / 2][tile.Coord.Y + DUNGEON_Y * Room.Y_LENGTH / 2] = tile;
            TileObjects.Add(tile.Coord, tileGo);
        }

        public void ClearTiles()
        {
            foreach (var tileObject in TileObjects.Values)
            {
                if (tileObject != null)
                {
                    GameObject.Destroy(tileObject);
                }
            }

            TileObjects.Clear();
            for (int i = 0; i < DUNGEON_X * Room.X_LENGTH; i++)
            {
                Tiles[i] = new Tile[DUNGEON_Y * Room.Y_LENGTH];
            }
        }

        public bool TryGetTile(int x, int y, out Tile tile)
        {
            Coord coord = new Coord(x, y);
            if (!HasTile(coord))
            {
                tile = default;
                return false;
            }

            tile = GetTile(x, y);
            return true;
        }

        public bool HasTile(Coord coord)
        {
            return TileObjects.ContainsKey(coord);
        }

        public Tile GetTile(int x, int y)
        {
            x += DUNGEON_X * Room.X_LENGTH / 2;
            y += DUNGEON_Y * Room.Y_LENGTH / 2;

            if (x < 0 || y < 0 || x >= DUNGEON_X * Room.X_LENGTH || y >= DUNGEON_Y * Room.Y_LENGTH)
                return default;

            Tile tile = Tiles[x][y];

            return tile;
        }

        public void SetTile(int x, int y, Tile tile)
        {
            Tiles[x + DUNGEON_X * Room.X_LENGTH / 2][y + DUNGEON_Y * Room.Y_LENGTH / 2] = tile;
        }

        public void SetOrRegisterTile(Tile tile)
        {
            if (HasTile(tile.Coord))
            {
                SetTile(tile.Coord.X, tile.Coord.Y, tile);
            }
            else
            {
                RegisterTile(tile);
            }
        }
        
        /// <summary>
        /// 활성화된 모든 방에서 랜덤한 빈 타일 위치를 반환
        /// </summary>
        /// <param name="maxAttempts">최대 시도 횟수</param>
        /// <returns>랜덤한 빈 타일의 좌표 (없으면 Coord.Zero 반환)</returns>
        public Coord GetRandomEmptyTileFromRooms(int maxAttempts)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // 활성화된 방 중 랜덤 선택
                Room randomRoom = GetRandomActiveRoom();
                if (randomRoom == null) break; // 활성화된 방이 없으면 중단

                // 방의 타일 중 랜덤 좌표 선택
                int randomX = Random.Range(-(Room.X_LENGTH / 2), Room.X_LENGTH / 2 + 1);
                int randomY = Random.Range(-(Room.Y_LENGTH / 2), Room.Y_LENGTH / 2 + 1);

                // 중심좌표 + 랜덤 오프셋으로 완전한 타일 좌표 계산
                int tileX = randomRoom.CenterPos.X + randomX;
                int tileY = randomRoom.CenterPos.Y + randomY;

                // 해당 타일이 비어 있는지 확인
                Tile tile = GetTile(tileX, tileY);
                if (tile.Status.HasFlag(StatusFlag.Empty))
                {
                    return new Coord(tileX, tileY);
                }
            }

            // 빈 타일을 찾지 못한 경우
            return Coord.Zero;
        }
        
        public Coord[] Offsets2Coords(Coord[] offsets, Tile curTile)
        {
            Coord[] coords = new Coord[offsets.Length];
            for (int i = 0; i < offsets.Length; i++)
            {
                coords[i] = new Coord(curTile.Coord.X + offsets[i].X, curTile.Coord.Y + offsets[i].Y);
            }

            return coords;
        }
    }
}
