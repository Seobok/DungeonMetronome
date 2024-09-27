using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    /// <summary>
    /// 방 하나를 관리하는 스크립트
    /// 각 방은 가로 11칸 세로 7칸을 가지고 있으며 이는 벽을 포함한 칸 수 이다.
    /// Room의 Position은 방의 좌측 하단이고 다음 방의 Position까지 x는 11, y 는 7 떨어져있다. 
    /// </summary>
    private Tile[,] tiles;
    public const int X = 11;
    public const int Y = 7;

    [HideInInspector] public int roomX;
    [HideInInspector] public int roomY;

    [SerializeField] private Room room_prefab;
    [SerializeField] private Tile tile_prefab;
    [SerializeField] private Block block_prefab;

    [HideInInspector] public Room[] adjacentRoom;

    /// <summary>
    /// 방의 타일과 인접 방 초기화
    /// 생성해야할 방 관련 프리팹 예외처리
    /// 타일 생성
    /// 이미지 생성 (벽 이미지문제로 인한 후 처리)
    /// </summary>
    private void Awake()
    {
        tiles = new Tile[X, Y];
        adjacentRoom = new Room[4] { null, null, null, null };

        if (room_prefab == null)
        {
            Debug.LogError("Tile Prefab Is Empty");
        }

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                Tile go;
                go = Instantiate(tile_prefab);

                go.transform.SetParent(transform);
                go.parentRoom = this;
                go.x = i;
                go.y = j;
                go.onTilePlayer = null;
                go.onTileUnit = null;

                tiles[i, j] = go;

                go.transform.localPosition = new Vector3(i, j, 0);
            }
        }
    }

    private void OnEnable()
    {
        foreach(var tile in tiles)
        {
            if(tile != null)
            {
                tile.onTilePlayer = null;
                tile.onTileUnit = null;
            }
        }
    }

    public void SetBlock()
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };
        foreach(var tile in tiles)
        {
            for(int i=0;i<4;i++)
            {
                if (GetTile(tile.x + dx[i], tile.y + dy[i]) == null)
                {
                    var blockGo = Instantiate(block_prefab);

                    blockGo.SetTile(tile);
                    blockGo.transform.position = blockGo.GetTile().transform.position;

                    break;
                }
            }
        }
    }

    /// <summary>
    /// 해당 방에서의 x, y 값을 기점으로 해당 타일을 가져오는 함수
    /// </summary>
    /// <param name="x">해당 방의 x값</param>
    /// <param name="y">해당 방의 y값</param>
    /// <returns>방의 범위를 넘어가도 타일을 가져오며, 방이없을 경우 null을 리턴</returns>
    public Tile GetTile(int x, int y)
    {
        if(x < 0)
        {
            if (adjacentRoom[3] != null)
            {
                return adjacentRoom[3].GetTile(X - 1, y);
            }
            return null;
        }
        else if(y < 0)
        {
            if (adjacentRoom[2] != null)
            {
                return adjacentRoom[2].GetTile(x, Y - 1);
            }
            return null;
        }
        else if(x >= X)
        {
            if (adjacentRoom[1] != null)
            {
                return adjacentRoom[1].GetTile(0, y);
            }
            return null;
        }
        else if(y >= Y)
        {
            if (adjacentRoom[0] != null)
            {
                return adjacentRoom[0].GetTile(x, 0);
            }
            return null;
        }

        return tiles[x, y];
    }

    /// <summary>
    /// 타일과 그 타일로부터의 위치정보를 기반으로 해당 범위의 타일을 가져오는 함수
    /// </summary>
    /// <param name="curTile">범위의 기점이 되는 타일</param>
    /// <param name="range">curTile로 부터 받아와야할 범위를 나타낼 타일 리스트</param>
    /// <returns>curTile로부터 range만큼 떨어진 타일의 List</returns>
    public List<Tile> GetTiles(Tile curTile, List<Vector2> range)
    {
        List<Tile> Range = new List<Tile>();
        foreach(Vector2 pos in range)
        {
            var tile = GetTile(curTile.x + (int)pos.x, curTile.y + (int)pos.y);
            if(tile != null)
            {
                Range.Add(tile);
            }
        }

        return Range;
    }
}
