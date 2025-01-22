using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class Dungeon
    {
        public Room[,] Rooms { get; private set; } = new Room[DUNGEON_X, DUNGEON_Y];
        public int RoomCount { get; private set; }
        public Room StartRoom { get; private set; }


        public const int DUNGEON_X = 15;
        public const int DUNGEON_Y = 15;
        private const int MAX_ROOM_COUNT = 150;

        private List<Room> _roomPool;
        private readonly Vector2[] _directions = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

        
        public Dungeon()
        {
            InitializeRooms();
        }

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
                Room newRoom = new Room();
                if (i == 0)
                {
                    StartRoom = newRoom;
                }

                newRoom.Dungeon = this;
                
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

            RoomCount = roomCount;
            
            //첫 방은 가운데 고정
            Room startRoom = StartRoom;
            startRoom.X = DUNGEON_X / 2;
            startRoom.Y = DUNGEON_Y / 2;
            Rooms[startRoom.X, startRoom.Y] = startRoom;
            startRoom.SpawnTiles();
            
            int setCount = 1;   //위치가 설정된 방의 갯수
            while (setCount < roomCount)
            {
                Room room = _roomPool[Random.Range(0, setCount)];
                Vector2 direction = _directions[Random.Range(0, _directions.Length)];
                
                int nextRoomX = room.X + (int)direction.x;
                int nextRoomY = room.Y + (int)direction.y;

                //Rooms의 배열 범위에 들어가는지 확인
                if (nextRoomX >= 0 && nextRoomY >= 0 && nextRoomX < DUNGEON_X && nextRoomY < DUNGEON_Y)
                {
                    if (Rooms[nextRoomX, nextRoomY] == null)
                    {
                        _roomPool[setCount].X = nextRoomX;
                        _roomPool[setCount].Y = nextRoomY;
                        Rooms[nextRoomX, nextRoomY] = _roomPool[setCount];

                        _roomPool[setCount].CenterPos = room.CenterPos + new Vector2(direction.x * Room.X_LENGTH, direction.y * Room.Y_LENGTH);
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
    }
}