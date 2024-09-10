using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 1.0f;
    private SpriteRenderer spriteRenderer;

    #region PlayerPosition
    /// <summary>
    /// Player의 위치 정보에 관한 변수
    /// 현재 위치하는 방의 정보
    /// 방에서의 X, Y 좌표
    /// </summary>
    private Room curRoom;
    private int room_x, room_y;
    public int Room_X
    {
        get { return room_x; }
        set 
        {
            if(value >= Room.X)
            {
                value -= Room.X;
                curRoom = curRoom.adjacentRoom[1];
                Debug.Log(curRoom.name);
            }
            if(value < 0)
            {
                value += Room.X;
                curRoom = curRoom.adjacentRoom[3];
                Debug.Log(curRoom.name);
            }
            room_x = value; 
        }
    }
    public int Room_Y
    {
        get { return room_y; }
        set
        {
            if (value >= Room.Y)
            {
                value -= Room.Y;
                curRoom = curRoom.adjacentRoom[0];
                Debug.Log(curRoom.name);
            }
            if (value < 0)
            {
                value += Room.Y;
                curRoom = curRoom.adjacentRoom[2];
                Debug.Log(curRoom.name);
            }
            room_y = value;
        }
    }
    #endregion
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMove(InputValue inputValue)
    {
        //임시 코드
        if(curRoom == null)
        {
            InitPlayerPosition();
            return;
        }
        var input = inputValue.Get<Vector2>();

        int nextXPos = Room_X + (int)input.x;
        int nextYPos = Room_Y + (int)input.y;

        var floorTile = curRoom.GetTile(nextXPos, nextYPos).GetComponent<Floor>();

        if (floorTile)
        {
            //Tile에 Object가 있으면 상호작용
            {

            }
            //이동
            transform.Translate(input.x, input.y, 0);
            Room_X = nextXPos;
            Room_Y = nextYPos;

            if (input.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (input.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
        
    }

    public void InitPlayerPosition()
    {
        Room_X = Room.X / 2;
        Room_Y = Room.Y / 2;
        curRoom = DungeonManager.instance.rooms[DungeonManager.DUNGEON_X / 2, DungeonManager.DUNGEON_Y / 2];
        transform.position = curRoom.GetTile(Room_X, Room_Y).transform.position;
    }
}
