using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    /// <summary>
    /// 던전의 한 층을 생성하는 스크립트
    /// GenerateRoom 코드를 사용하면 ROOM_CNT 만큼의 방이 생성됨
    /// </summary>
    [SerializeField] private Room room_prefabs;
    [SerializeField] private Goal goal_prefabs;
    [HideInInspector] public Room[,] rooms;
    [HideInInspector] public List<Room> roomList;
    int[] dirX = new int[4] { 0, 1, 0, -1 };
    int[] dirY = new int[4] { 1, 0, -1, 0 };

    [HideInInspector] public Room lobby;

    public static DungeonManager instance = null;

    public const int DUNGEON_X = 5;
    public const int DUNGEON_Y = 5;
    const int MAX_ROOM = DUNGEON_X * DUNGEON_Y;
    const int ROOM_CNT = 15;

    private void Awake()
    {

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        rooms = new Room[DUNGEON_X, DUNGEON_Y];
    }

    private void Start()
    {
        
    }

    public void GenerateLobby()
    {
        if(lobby == null)
        {
            var lobbyRoom = Instantiate(room_prefabs);
            lobby = lobbyRoom;

            lobby.SetBlock();

            if(goal_prefabs != null)
            {
                var goal = Instantiate(goal_prefabs);

                goal.RoomX = 7;
                goal.RoomY = 3;
                goal.curRoom = lobby;
                goal.transform.position = goal.GetTile().transform.position;
                goal.GetTile().onTileUnit = goal;

                goal.transform.SetParent(goal.GetTile().transform);
            }
        }
    }

    public void DeactiveLobby()
    {
        lobby.gameObject.SetActive(false);
    }

    public void GenerateRoom()
    {
        //시작 지점 만들기
        var startRoom = Instantiate(room_prefabs);
        rooms[DUNGEON_X / 2, DUNGEON_Y / 2] = startRoom;
        startRoom.roomX = DUNGEON_X / 2;
        startRoom.roomY = DUNGEON_Y / 2;
        startRoom.name = "startRoom";

        //랜덤으로 방을 선택할 수 있도록 보유중인 방을 리스트에 임시 저장
        roomList = new List<Room>();
        roomList.Add(startRoom);

        //특정 갯수의 방이 만들어 질때까지 반복
        while (roomList.Count != ROOM_CNT)
        {
            var rand = Random.Range(0, roomList.Count);
            var dir = Random.Range(0, 4);

            Room curRoom = roomList[rand];
            int nextRoomX = curRoom.roomX + dirX[dir];
            int nextRoomY = curRoom.roomY + dirY[dir];

            if (curRoom.adjacentRoom[dir] == null && nextRoomX >= 0 && nextRoomX < DUNGEON_X && nextRoomY >= 0 && nextRoomY < DUNGEON_Y)
            {
                //방 생성 가능
                var newRoom = Instantiate(room_prefabs);
                newRoom.name = $"Room {roomList.Count}";

                //배열 설정
                newRoom.roomX = nextRoomX;
                newRoom.roomY = nextRoomY;

                rooms[newRoom.roomX, newRoom.roomY] = newRoom;

                //위치 조정
                newRoom.transform.SetParent(transform.parent);
                switch (dir)
                {
                    case (int)E_Dir.ED_Up:
                        newRoom.transform.position = curRoom.transform.position + new Vector3(0, Room.Y, 0);
                        break;
                    case (int)E_Dir.ED_Right:
                        newRoom.transform.position = curRoom.transform.position + new Vector3(Room.X, 0, 0);
                        break;
                    case (int)E_Dir.ED_Down:
                        newRoom.transform.position = curRoom.transform.position + new Vector3(0, -Room.Y, 0);
                        break;
                    case (int)E_Dir.ED_Left:
                        newRoom.transform.position = curRoom.transform.position + new Vector3(-Room.X, 0, 0);
                        break;
                }

                //방 연결
                for (int i = 0; i < 4; i++)
                {
                    int adjacentRoomX = newRoom.roomX + dirX[i];
                    int adjacentRoomY = newRoom.roomY + dirY[i];
                    if (adjacentRoomX >= 0 && adjacentRoomY >= 0 && adjacentRoomX < DUNGEON_X && adjacentRoomY < DUNGEON_Y)
                    {
                        var adjacentRoom = rooms[adjacentRoomX, adjacentRoomY];
                        if (adjacentRoom != null)
                        {
                            adjacentRoom.adjacentRoom[(i + 2) % 4] = newRoom;
                            newRoom.adjacentRoom[i] = adjacentRoom;
                        }
                        else
                        {
                            newRoom.adjacentRoom[i] = null;
                        }
                    }
                }

                roomList.Add(newRoom);
            }
        }

        //마지막으로 생성된 방에 도착점 생성
        GenerateGoal();

        //외벽 매꾸기
        foreach(var room in roomList)
        {
            room.SetBlock();
        }
    }

    public void GenerateGoal()
    {
        if (goal_prefabs)
        {
            //(1,1) ~ (9,5)
            var rand_x = Random.Range(1, Room.X - 1);
            var rand_y = Random.Range(1, Room.Y - 1);

            var goal = Instantiate(goal_prefabs);

            goal.RoomX = rand_x;
            goal.RoomY = rand_y;
            goal.curRoom = roomList[ROOM_CNT - 1];
            goal.transform.position = goal.GetTile().transform.position;
            goal.GetTile().onTileUnit = goal;

            goal.transform.SetParent(goal.GetTile().transform);
        }
    }
}
