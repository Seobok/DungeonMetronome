using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Tile[,] tiles;
    public const int X = 11;
    public const int Y = 7;

    [SerializeField] private Floor floor_prefab;
    [SerializeField] private Wall wall_prefab;

    private void Start()
    {
        if(floor_prefab == null || wall_prefab == null)
        {
            Debug.LogError("Tile Prefab Is Empty");
        }
        tiles = new Tile[X, Y];
        for(int i=0; i< tiles.GetLength(0); i++)
        {
            for(int j = 0;j< tiles.GetLength(1); j++)
            {
                Tile go;
                if(i == 0 || j == 0 || i == X - 1 || j == Y - 1)
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
