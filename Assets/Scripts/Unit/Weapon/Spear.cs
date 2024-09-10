using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
    private void Awake()
    {
        damage = 1;
    }
    public override List<Tile> GetRange(Tile forwardTile, Vector2 dir)
    {
        List<Tile> range = new List<Tile>();

        range.Add(forwardTile);
        range.Add(forwardTile.parentRoom.GetTile((int)(forwardTile.x + dir.x), (int)(forwardTile.y + dir.y)));
        return range;
    }
}
