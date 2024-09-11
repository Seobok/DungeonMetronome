using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    private int detaceRange = 3;
    private bool bIsReadyMove = false;
    private bool bIsReadyAttack = false;

    public void Act()
    {
        if(bIsReadyAttack)
        {
            Attack();
        }
        if(bIsReadyMove)
        {
            Move();
        }
        if (Detact())
        {

        }
    }

    public bool Detact()
    {
        //Setting Range
        List<Vector2> list = new List<Vector2>();
        for (int i = -detaceRange; i <= detaceRange; i++)
        {
            for (int j = -detaceRange; j <= detaceRange; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) > detaceRange) continue;

                list.Add(new Vector2(i, j));
            }
        }

        //Detect Tiles
        var tiles = curRoom.GetTiles(GetTile(), list);
        foreach (var tile in tiles)
        {
            if (tile.bisOnTilePlayer)
                return true;
        }

        return false;
    }

    public virtual void Attack()
    {
        Debug.Log("Attack");
    }

    public virtual void Move()
    {
        Debug.Log("Move");
    }
}
