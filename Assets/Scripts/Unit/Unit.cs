using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    #region Position Field
    /// <summary>
    /// Unit의 위치 정보에 관한 변수
    /// 현재 위치하는 방의 정보
    /// 방에서의 X, Y 좌표
    /// </summary>

    [Header("Position")]
    public Room curRoom;
    [SerializeField] private int _roomX, _roomY;
    public int RoomX
    {
        get { return _roomX; }
        set
        {
            if (value >= Room.X)
            {
                value -= Room.X;
                curRoom = curRoom.adjacentRoom[1];
            }
            if (value < 0)
            {
                value += Room.X;
                curRoom = curRoom.adjacentRoom[3];
            }
            _roomX = value;
        }
    }
    public int RoomY
    {
        get { return _roomY; }
        set
        {
            if (value >= Room.Y)
            {
                value -= Room.Y;
                curRoom = curRoom.adjacentRoom[0];
            }
            if (value < 0)
            {
                value += Room.Y;
                curRoom = curRoom.adjacentRoom[2];
            }
            _roomY = value;
        }
    }

    public Tile GetTile()
    {
        return curRoom.GetTile(RoomX, RoomY);
    }

    public void SetTile(Tile tile)
    {
        if (tile.onTileUnit != null) return;

        if(curRoom != null)
        {
            GetTile().onTileUnit = null;
        }

        curRoom = tile.parentRoom;
        RoomX = tile.x;
        RoomY = tile.y;

        GetTile().onTileUnit = this;

        transform.SetParent(tile.transform);
    }
    #endregion
}
