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

    [SerializeField] private Floor floor_prefab;
    [SerializeField] private Wall wall_prefab;
    [SerializeField] private Room room_prefab;

    [HideInInspector] public Room[] adjacentRoom;

    private void Awake()
    {
        tiles = new Tile[X, Y];
        adjacentRoom = new Room[4] { null, null, null, null };
    }

    private void Start()
    {
        if(floor_prefab == null || wall_prefab == null || room_prefab == null)
        {
            Debug.LogError("Tile Prefab Is Empty");
        }
        
        for(int i=0; i< tiles.GetLength(0); i++)
        {
            for(int j = 0;j< tiles.GetLength(1); j++)
            {
                //문 위치
                if ((i == 0 && j == Y / 2) || (i == X - 1 && j == Y / 2) || (i == X / 2 && j == 0) || (i == X / 2 && j == Y - 1))
                    continue;

                Tile go;
                //벽 위치
                if (i == 0 || j == 0 || i == X - 1 || j == Y - 1)
                {
                    go = Instantiate(wall_prefab);
                }
                else
                {
                    go = Instantiate(floor_prefab);
                }

                go.transform.SetParent(transform);
                go.parentRoom = this;
                go.x = i;
                go.y = j;

                tiles[i, j] = go;

                go.transform.localPosition = new Vector3(i, j, 0);
            }
        }

        //타일이 전체 생성된 이후 sprite 다시 생성
        for(int i=0;i< tiles.GetLength(0);i++)
        {
            for(int j=0;j<tiles.GetLength(1);j++)
            {
                if (tiles[i,j] == null)
                    continue;
                Wall go = tiles[i, j].GetComponent<Wall>();
                if(go != null)
                    go.SetWallSprite();
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= X || y >= Y)
            return null;

        return tiles[x, y];
    }
}
