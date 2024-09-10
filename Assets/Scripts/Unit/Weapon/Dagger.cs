using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    private void Awake()
    {
        damage = 1;
    }
    public override List<Tile> GetRange(Tile forwardTile)
    {
        List<Tile> range = new List<Tile>();
        range.Add(forwardTile);
        return range;
    }
}
